using Titan.UFC.Users.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Titan.UFC.Users.WebAPI.Services
{
    public interface IUserService
    {
        Task<object> CreateAsync(User user);
        Task<User> ReadOneAsync(int userId);
        Task UpdateAsync(UserUpdate user);
        Task RemoveAsync(int userId);
        Task<bool> IsExistAsync(int userId);
        Task<ForgotPassword> CreateAsync(ForgotPassword forgotPassword);
        Task<bool> ResetPasswordAsync(ResetPassword resetPassword);
        // To be Removed from Forget Password.
        Task DeleteForgotPasswordOTPAsync(int userId);
    }
}
