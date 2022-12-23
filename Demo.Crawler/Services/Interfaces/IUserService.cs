using Demo.CoreData.Models;

namespace Demo.Crawler.Services.Interfaces;

public interface IUserService
{
    User? GetUserByUserName(string? userName);
}