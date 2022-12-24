using JSN.CoreData.Models;
using JSN.CoreData.ViewModels;
using JSN.Crawler.Common;

namespace JSN.Crawler.Services.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(UserView request);
    Task<TokenModel> LoginAsync(UserView request);
    Task<TokenModel> RefreshTokenAsync(TokenModel? tokenModel);
    string CheckLogin(UserView request);
    string CheckRefreshToken(TokenModel? tokenModel);
}