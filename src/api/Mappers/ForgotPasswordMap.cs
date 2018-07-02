using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Titan.UFC.Users.WebAPI.Entities;

namespace Titan.UFC.Users.WebAPI.Mappers
{
    public class ForgotPasswordMap
    {
        public ForgotPasswordMap(EntityTypeBuilder<ForgotPasswordEntity> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ForgotPasswordID);
            entityBuilder.Property(t => t.UserID);
            entityBuilder.Property(t => t.ResetOTP);
            entityBuilder.Property(t => t.OTPExpiryTime);
            entityBuilder.Property(t => t.CreatedDate);
            entityBuilder.Property(t => t.ResetTime);
        }
    }
}
