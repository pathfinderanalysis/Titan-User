using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Titan.UFC.Users.WebAPI.Entities;

namespace Titan.UFC.Users.WebAPI.Mappers
{
    public class UserEmailMap
    {
        public UserEmailMap(EntityTypeBuilder<UserEmailEntity> entityBuilder)
        {
            entityBuilder.HasKey(t => t.UserEmailID);
            entityBuilder.HasOne<UserEntity>(u => u.User).WithOne(e => e.UserEmail).HasForeignKey<UserEmailEntity>(u => u.UserID).OnDelete(DeleteBehavior.Cascade);
            entityBuilder.Property(t => t.EmailAddress).IsRequired();
            entityBuilder.Property(t => t.IsVerified);
            entityBuilder.Property(t => t.VerifiedDate);
            entityBuilder.Property(t => t.VerificationKey).IsRequired();
            entityBuilder.Property(t => t.CreatedDate).IsRequired();          
        }
    }
}
