using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Demo.CoreData.Entities;
using Demo.CoreData.Models;
using Demo.CoreData.ViewModels;
using Demo.Crawler.Common;
using Demo.Crawler.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Crawler.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;
    private readonly IUserService _userService;

    public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration, IUserService userService,
        IRepository<User> userRepository)
    {
        _configuration = configuration;
        _userService = userService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<string> GetMe()
    {
        var UserName = _userService.GetMyName();
        return Ok(UserName);
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserView request)
    {
        CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

        User newUser = new()
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            IsActive = true,
            Role = "Member"
        };

        _userRepository.Add(newUser);
        await _unitOfWork.CommitAsync();

        return Ok(newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenModel>> LoginAsync(UserView request)
    {
        var user = _userService.GetUserByUserName(request.UserName);
        if (user == null) return BadRequest("User not found.");

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Wrong password.");

        var token = CreateToken(user);
        var refreshToken = GenerateRefreshToken();
        SetRefreshToken(user, refreshToken);

        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return Ok(new TokenModel
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token
        });
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenModel>> RefreshToken(TokenModel tokenModel)
    {
        //var refreshToken = Request.Cookies["refreshToken"];

        if (tokenModel is null) return BadRequest("Invalid client request");

        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null) return BadRequest("Invalid access token");

        var userName = principal.Identity?.Name;
        var user = _userService.GetUserByUserName(userName);
        if (user == null) return BadRequest("Invalid access token");
        if (user.RefreshToken != refreshToken) return BadRequest("Invalid refresh token");
        if (user.TokenExpires <= DateTime.Now) return BadRequest("Token expired");

        var newAccessToken = CreateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        SetRefreshToken(user, newRefreshToken);

        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return Ok(new TokenModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token
        });
    }

    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };

        return refreshToken;
    }

    private void SetRefreshToken(User user, RefreshToken newRefreshToken)
    {
        //var cookieOptions = new CookieOptions
        //{
        //    HttpOnly = true,
        //    Expires = newRefreshToken.Expires
        //};
        //Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("JWT:Token").Value));
        var tokenValidityInMinutes = int.Parse(_configuration.GetSection("JWT:TokenValidityInMinutes").Value);

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
                StringComparison.InvariantCultureIgnoreCase)) return null;

        return principal;
    }
}