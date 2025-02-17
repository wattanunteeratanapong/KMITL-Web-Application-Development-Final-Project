using Togeta.Models;

namespace Togeta.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user);
        Task<User?> LoginUserAsync(string username, string password);  // Add this method
        Task<User> GetUserByUsernameAsync(string username);
    }
}
