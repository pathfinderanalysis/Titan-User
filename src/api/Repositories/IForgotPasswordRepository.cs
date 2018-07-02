using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Repositories
{
    public interface IForgotPasswordRepository
    {
        Task<ForgotPassword> CreateForgotPasswordOTPAsync(ForgotPassword forgotPassword);
        Task<bool> ResetPasswordAsync(string otp, string newPassword);

        Task RemoveForgotPasswordAsync(int userId);
    }
}
