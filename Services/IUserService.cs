using Togeta.Models;

namespace Togeta.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user);
    }
}