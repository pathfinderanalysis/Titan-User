using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Repositories
{
    public  interface IEmailRepository
    {
        Task<Email> ReadOneAsync(int userId);
        Task RemoveAsync(int userId);
        Task<int> CheckValidUserByEmailAsync(Email email);
    }
}
