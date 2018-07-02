using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Services
{
   public interface IContactsService
    {
        Task<Phone> ReadOnePhoneAsync(int userId);
        Task RemovePhoneAsync(int userId);

        Task<Email> ReadOneEmailAsync(int userId);
        Task RemoveEmailAsync(int userId);
        Task<int> CheckValidUserByPhoneAsync(Phone phone);
        Task<int> CheckValidUserByEmailAsync(Email email);
    }
}
