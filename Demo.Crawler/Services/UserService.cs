using System.Security.Claims;
using Demo.Crawler.Services.Interfaces;
using Demo.CoreData.Models;
using Demo.CoreData.Entities;

namespace Demo.Crawler.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _userRepository;

        public UserService(IHttpContextAccessor httpContextAccessor, IRepository<User> userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return result;
        }

        public User? GetUserByUserName(string userName)
        {
            var user = _userRepository.List(x => x.UserName == userName).SingleOrDefault();
            return user;
        }
    }
}
