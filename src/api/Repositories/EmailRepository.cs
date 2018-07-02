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
    public class EmailRepository : IEmailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<UserEmailEntity> _emailEntity;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public EmailRepository(ApplicationDbContext context, IMapper mapper, ILogger<EmailRepository> logger)
        {
            _context = context;
            _emailEntity = _context.Set<UserEmailEntity>();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Email> ReadOneAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {userId}", userId);

            var emailEntity = await _emailEntity
                .Where(u => u.UserID == userId).FirstOrDefaultAsync();

            return _mapper.Map<Email>(emailEntity);
        }

        public async Task RemoveAsync(int userId)
        {
            var emailEntity = await _emailEntity
             .Where(u => u.UserID == userId).FirstOrDefaultAsync();

            if (emailEntity == null)
                throw new ArgumentException("email not found");

            _context.Remove<UserEmailEntity>(emailEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CheckValidUserByEmailAsync(Email email)
        {
            _logger.LogInformation(LoggerEvents.IsExistItem, "Is exist item {emailAddress}", email.Address);

            var emailEntity = await _emailEntity
               .Where(u => u.EmailAddress == email.Address)

               .FirstOrDefaultAsync();

            if (emailEntity == null)
                throw new ArgumentException("email not found");
            return emailEntity.UserID;
        }
    }
}
