using Demo.CoreData.Models;
namespace Demo.Crawler.Services.Interfaces
{
    public interface IUserService
    {
        string GetMyName();
        User? GetUserByUserName(string userName);
    }
}
