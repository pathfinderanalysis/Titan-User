using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Services
{
    public interface IAddressService
    {
        Task<Address> CreateAsync(int userId, Address address);
        Task<List<Address>> ReadAsync(int userId);
        Task UpdateAsync(int userid,List<Address> address);
        Task<Address> ReadOneAsync(int userId, int addressId);
        Task UpdateAsync(int userId, int addressId, Address address);

    }
}
