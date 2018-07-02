using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Repositories;

namespace Titan.UFC.Users.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IForgotPasswordRepository _forgotPasswordRepository;
        private readonly ILogger _logger;

        public UserService(IUserRepository userRepository, IForgotPasswordRepository forgotPasswordRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(UserRepository));
            _forgotPasswordRepository = forgotPasswordRepository ?? throw new ArgumentNullException(nameof(ForgotPasswordRepository));
            _logger = logger;
        }

        public async Task<object> CreateAsync(User user)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "Insert item {User}", JsonConvert.SerializeObject(user));

            user.AuthRefID = Guid.NewGuid();
            return await _userRepository.CreateAsync(user);
        }

        public async Task<User> ReadOneAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {userId}", userId);

            var user = await this._userRepository.ReadOneAsync(userId);
            return user;
        }

        public async Task UpdateAsync(UserUpdate user)
        {
            _logger.LogInformation(LoggerEvents.UpdateItem, "Update item {User}", JsonConvert.SerializeObject(user));

            await this._userRepository.UpdateAsync(user);
        }

        public async Task RemoveAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.DeleteItem, "Remove item {userId}", userId);

            await this._userRepository.DeleteAsync(userId);
        }

        public async Task<bool> IsExistAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.IsExistItem, "Is exist item {userId}", userId);

            return await this._userRepository.IsExistAsync(userId);
        }

        public async Task<ForgotPassword> CreateAsync(ForgotPassword forgotPassword)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "Insert item {0}", JsonConvert.SerializeObject(forgotPassword));            
            return await _forgotPasswordRepository.CreateForgotPasswordOTPAsync(forgotPassword);
        }

        public async Task<bool> ResetPasswordAsync(ResetPassword resetPassword)
        {
            _logger.LogInformation(LoggerEvents.UpdateItem, "Reset Password ({0},{1})", resetPassword.OTP, resetPassword.NewPassword);
            return await _forgotPasswordRepository.ResetPasswordAsync(resetPassword.OTP, resetPassword.NewPassword);
        }

        public async Task DeleteForgotPasswordOTPAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.DeleteItem, "Delete Exist Data from ForgetPassword ({0})", userId);
            await _forgotPasswordRepository.RemoveForgotPasswordAsync(userId);

        }
    }
}
