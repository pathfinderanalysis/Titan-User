using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Entities;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<UserPhoneEntity> _phoneEntity;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PhoneRepository(ApplicationDbContext context, IMapper mapper, ILogger<PhoneRepository> logger)
        {
            _context = context;
            _phoneEntity = _context.Set<UserPhoneEntity>();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Phone> ReadOneAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {userId}", userId);

            var phoneEntity = await _phoneEntity
                .Where(u => u.UserID == userId).FirstOrDefaultAsync();

            return _mapper.Map<Phone>(phoneEntity);
        }

        public async Task RemoveAsync(int userId)
        {
            var phoneEntity = await _phoneEntity
              .Where(u => u.UserID == userId).FirstOrDefaultAsync();

            if (phoneEntity == null)
                throw new ArgumentException("phone not found");

            _context.Remove<UserPhoneEntity>(phoneEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CheckValidUserByPhoneAsync(Phone phone)
        {
            _logger.LogInformation(LoggerEvents.IsExistItem, "Is exist item {Phonenumber}", phone.Number);

            var phoneEntity = await _phoneEntity
               .Where(u => u.CallingCode == phone.CountryCode && u.PhoneNumber == phone.Number)

               .FirstOrDefaultAsync();

            if (phoneEntity == null)
                throw new ArgumentException("phone not found");

            return phoneEntity.UserID;
        }
    }
}
