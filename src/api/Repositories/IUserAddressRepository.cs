using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Repositories
{
    public interface IUserAddressRepository
    {
        Task<Address> CreateAsync(int userId, Address address);
        Task<List<Address>> ReadAsync(int userId);
        Task UpdateAsync(int userid,List<Address> addresses);
        Task<Address> ReadOneAsync(int userId, int addressId);
        Task UpdateAsync(int userId, int addressId, Address address);
    }
}
