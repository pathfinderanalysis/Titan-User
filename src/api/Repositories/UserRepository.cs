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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<UserEntity> _userEntity;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserRepository(ApplicationDbContext context, IMapper mapper, ILogger<UserRepository> logger)
        {
            _context = context;
            _userEntity = _context.Set<UserEntity>();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<object> CreateAsync(User user)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "Insert item {user}", JsonConvert.SerializeObject(user));
            if (user == null)
                throw new ArgumentNullException("user");

            var userEntity = _mapper.Map<UserEntity>(user);
            userEntity.UserAddresses = _mapper.Map <List<UserAddressEntity>>(user.Addresses); 

            _context.Add<UserEntity>(userEntity);
            await _context.SaveChangesAsync();

            return new { Id = userEntity.UserID, AuthRefId = userEntity.AuthRefID, PhotoUri = userEntity.PhotoUri };
        }

        public async Task<User> ReadOneAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {userId}", userId);

            var userEntity = await _userEntity
                .Where(u => u.UserID == userId)
                .Include(a => a.UserAddresses)
                .ThenInclude(a => a.Address)
                .Include(e => e.UserEmail)
                .Include(e => e.UserPhone)
                .FirstOrDefaultAsync();

            return _mapper.Map<User>(userEntity);
        }

        public async Task UpdateAsync(UserUpdate user)
        {
            _logger.LogInformation(LoggerEvents.UpdateItem, "Update item {user}", JsonConvert.SerializeObject(user));

            if (user == null)
                throw new ArgumentNullException("userEntity");

            var userEntity = _mapper.Map<UserEntity>(user);
            userEntity.UserAddresses = _mapper.Map<List<UserAddressEntity>>(user.Addresses);
            userEntity.UserAddresses.ToList().ForEach(i => { i.UserID = user.UserID; i.User = _mapper.Map<UserEntity>(user); });         

            _context.Update<UserEntity>(userEntity).Property(x => x.CreatedDate).IsModified = false;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.DeleteItem, "Delete item {userId}", userId);

            var userEntity = await _userEntity
                .Where(u => u.UserID == userId)
                .Include(a => a.UserAddresses)
                .ThenInclude(a => a.Address)
                .Include(e => e.UserEmail)
                .Include(e => e.UserPhone)
                .FirstOrDefaultAsync();

            if (userEntity == null)
                throw new ArgumentException("user not found");

            var addresses = userEntity.UserAddresses.Select(i => i.Address);

            _context.Remove<UserEntity>(userEntity);
            _context.RemoveRange(addresses);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.IsExistItem, "Is exist item {userId}", userId);

            return await _userEntity.AnyAsync(x => x.UserID == userId);
        }
    }
}
