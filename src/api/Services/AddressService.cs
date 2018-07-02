using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Mappers;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Repositories;

namespace Titan.UFC.Users.WebAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly ILogger _logger;

        public AddressService(IUserAddressRepository userAddressRepository, ILogger<AddressService> logger)
        {
            _userAddressRepository = userAddressRepository ?? throw new ArgumentNullException(nameof(UserAddressRepository)); ;
            _logger = logger;
        }

        public async Task<Address> CreateAsync(int userId,Address address)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "Insert item {Address}", JsonConvert.SerializeObject(address));

            return await _userAddressRepository.CreateAsync(userId, address);
        }

        public async Task<Address> ReadOneAsync(int userId, int addressId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting address by userId {userId},{addressId}", userId,addressId);

            var address = await _userAddressRepository.ReadOneAsync(userId, addressId);
            return address;
        }

        public async Task<List<Address>> ReadAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting address {userId}", userId);

            var address = await this._userAddressRepository.ReadAsync(userId);
            return address;
        }
        public async Task UpdateAsync(int userId, List<Address> address)
        {
            _logger.LogInformation(LoggerEvents.UpdateItem, "Update userAddresses : ({userId},{addresses})", userId, JsonConvert.SerializeObject(address));
            await _userAddressRepository.UpdateAsync(userId, address);
        }

        public async Task UpdateAsync(int userId,int addressId,Address address)
        {
            _logger.LogInformation(LoggerEvents.UpdateItem, "Update userAddress : ({userId},{addressId},{address})", userId,addressId, JsonConvert.SerializeObject(address));
            await _userAddressRepository.UpdateAsync(userId,addressId, address);
        }
    }
}
