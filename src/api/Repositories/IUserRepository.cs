using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Repositories
{
    public interface IUserRepository
    {
        Task<object> CreateAsync(User user);
        Task<User> ReadOneAsync(int userId);
        Task UpdateAsync(UserUpdate user);
        Task DeleteAsync(int userId);
        Task<bool> IsExistAsync(int userId);
    }
}
