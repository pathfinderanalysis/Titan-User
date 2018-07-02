using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Titan.UFC.Users.WebAPI.Entities;

namespace Titan.UFC.Users.WebAPI.Mappers
{
    public class UserPhoneMap
    {
        public UserPhoneMap(EntityTypeBuilder<UserPhoneEntity> entityBuilder)
        {
            entityBuilder.HasKey(t => t.UserPhoneID);
            entityBuilder.HasOne<UserEntity>(u => u.User).WithOne(p => p.UserPhone).HasForeignKey<UserPhoneEntity>(u => u.UserID).OnDelete(DeleteBehavior.Cascade);
            entityBuilder.Property(t => t.PhoneNumber).IsRequired();
            entityBuilder.Property(t => t.CallingCode);
            entityBuilder.Property(t => t.IsVerified);
            entityBuilder.Property(t => t.VerifiedDate);
            entityBuilder.Property(t => t.VerificationKey).IsRequired();           
        }
    }
}
