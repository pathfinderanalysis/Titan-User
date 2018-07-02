using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Repositories
{
   public interface IPhoneRepository
    {
        Task<Phone> ReadOneAsync(int userId);
        Task RemoveAsync(int userId);
        Task<int> CheckValidUserByPhoneAsync(Phone phone);
    }
}
