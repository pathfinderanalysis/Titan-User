using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Entities;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Repositories
{
    public class ForgotPasswordRepository : IForgotPasswordRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<ForgotPasswordEntity> _forgotPasswordEntity;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ForgotPasswordRepository(ApplicationDbContext context, IMapper mapper, ILogger<ForgotPasswordRepository> logger)
        {
            _context = context;
            _forgotPasswordEntity = _context.Set<ForgotPasswordEntity>();
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ForgotPassword> CreateForgotPasswordOTPAsync(ForgotPassword forgotPassword)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "Insert item {forgotpassword}", JsonConvert.SerializeObject(forgotPassword));

            if (forgotPassword == null)
                throw new ArgumentNullException("forgotpassword");

            var forgotPasswordEntity = _mapper.Map<ForgotPasswordEntity>(forgotPassword);

            _context.Add<ForgotPasswordEntity>(forgotPasswordEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ForgotPassword>(forgotPasswordEntity);
        }

        public async Task<bool> ResetPasswordAsync(string otp, string newPassword)
        {
            _logger.LogInformation(LoggerEvents.UpdateItem, "Update item ({0},{1})", otp, newPassword);

            var forgotPassword = await _forgotPasswordEntity
                .Where(u => u.ResetOTP == otp && DateTime.UtcNow <= u.OTPExpiryTime && u.ResetTime == null).FirstOrDefaultAsync();
            if (forgotPassword != null)
            {
                var forgotPasswordEntity = _mapper.Map<ForgotPasswordEntity>(forgotPassword);
                forgotPasswordEntity.ResetTime = DateTime.UtcNow;

                _context.Update<ForgotPasswordEntity>(forgotPasswordEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task RemoveForgotPasswordAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.DeleteItem, "Remove item ({0})", userId);

            var forgetPasswordEntity = _forgotPasswordEntity.Where(x => x.UserID == userId);

            foreach (var forgetPassword in forgetPasswordEntity)
            {
                _context.Remove<ForgotPasswordEntity>(forgetPassword);
            }
            await _context.SaveChangesAsync();
        }
    }
}
