using Demo.CoreData.Entities;
using Demo.CoreData.Models;
using Demo.Crawler.Services.Interfaces;

namespace Demo.Crawler.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public User? GetUserByUserName(string? userName)
    {
        var user = _userRepository.List(x => x.UserName == userName).SingleOrDefault();
        return user;
    }
}