using JSN.CoreData.Models;
using JSN.CoreData.ViewModels;
using JSN.Crawler.Common;
using JSN.Crawler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JSN.Crawler.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserView request)
    {
        var newUser = await _authService.RegisterAsync(request);

        return Ok(newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenModel>> Login(UserView request)
    {
        var error = _authService.CheckLogin(request);

        if (!error.IsNullOrEmpty())
            return BadRequest(error);

        var newToken = await _authService.LoginAsync(request);

        return Ok(newToken);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenModel>> RefreshToken(TokenModel? tokenModel)
    {
        var error = _authService.CheckRefreshToken(tokenModel);

        if (!error.IsNullOrEmpty())
            return BadRequest(error);

        var newToken = await _authService.RefreshTokenAsync(tokenModel);

        return Ok(newToken);
    }
}