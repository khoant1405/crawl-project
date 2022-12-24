using JSN.CoreData.Models;

namespace JSN.Crawler.Services.Interfaces;

public interface IUserService
{
    User? GetUserByUserName(string? userName);
}