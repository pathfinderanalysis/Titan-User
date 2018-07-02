using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Entities;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Repositories
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<UserAddressEntity> _userAddressEntity;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserAddressRepository(ApplicationDbContext context, IMapper mapper, ILogger<UserAddressRepository> logger)
        {
            _context = context;
            _userAddressEntity = _context.Set<UserAddressEntity>();
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Address> CreateAsync(int userId, Address address)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "Insert item {address}", JsonConvert.SerializeObject(address));
            if (address == null)
                throw new ArgumentNullException("address");

            var addressEntity = _mapper.Map<AddressEntity>(address);
            addressEntity.UserAddress = _mapper.Map<UserAddressEntity>(address);
            addressEntity.UserAddress.UserID = userId;
            _context.Add<AddressEntity>(addressEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<Address>(addressEntity);
        }

        public async Task<Address> ReadOneAsync(int userId, int addressId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting user Address by userId and addressId: {userId},{addressId}", userId,addressId);
            var addressEntity = await _userAddressEntity
             .Where(i => i.UserID == userId && i.AddressID == addressId)
             .Include(a => a.Address)
             .Select(a => a.Address).FirstAsync();

            return _mapper.Map<Address>(addressEntity);
        }

        public async Task<List<Address>> ReadAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting addresses of userId: {userId}", userId);

            var addressEntity = await _userAddressEntity
                .Where(i => i.UserID == userId)
                .Include(a => a.Address)
                .Select(a => a.Address)
                .ToListAsync();

            return _mapper.Map<List<Address>>(addressEntity);
        }

        public async Task UpdateAsync(int userId, List<Address> addresses)
        {
            _logger.LogInformation(LoggerEvents.UpdateItem, "Update address : {userId} {addresses}", userId, JsonConvert.SerializeObject(addresses));

            if (userId == 0 || addresses == null)
                throw new ArgumentNullException("UserAddressEntity");

            foreach (var address in addresses)
            {
                var userAddressEntity = _mapper.Map<UserAddressEntity>(address);
                userAddressEntity.UserID = userId;
                userAddressEntity.Address.UpdatedDate = DateTime.UtcNow;
                _context.Update<UserAddressEntity>(userAddressEntity).Property(x => x.CreatedDate).IsModified = false;
                _context.Update(userAddressEntity.Address).Property(x => x.CreatedDate).IsModified = false;  // Should not Update the Address Created Date. 
            }
            await _context.SaveChangesAsync();

        }


        public async Task UpdateAsync(int userId,int addressId,Address address)
        {
            _logger.LogInformation(LoggerEvents.UpdateItem, "Update user address : {userId},{addressId} {address}", userId,addressId, JsonConvert.SerializeObject(address));

            if (userId==0|| addressId==0 || address==null)
            {
                throw new ArgumentNullException("Address is null");
            }

            var userAddressEntity = _mapper.Map<UserAddressEntity>(address);
            userAddressEntity.UserID = userId;
            userAddressEntity.Address.UpdatedDate = DateTime.UtcNow;
            _context.Update(userAddressEntity).Property(x => x.CreatedDate).IsModified = false;
            _context.Update(userAddressEntity.Address).Property(x => x.CreatedDate).IsModified = false;// Should not Update the Address Created Date
            await _context.SaveChangesAsync();
        }


    }
}
