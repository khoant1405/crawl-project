using Demo.CoreData.Models;
using Demo.CoreData.ViewModels;
using Demo.Crawler.Common;

namespace Demo.Crawler.Services.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(UserView request);
    Task<TokenModel> LoginAsync(UserView request);
    Task<TokenModel> RefreshTokenAsync(TokenModel? tokenModel);
    string CheckLogin(UserView request);
    string CheckRefreshToken(TokenModel? tokenModel);
}