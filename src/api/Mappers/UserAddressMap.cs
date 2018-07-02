using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Titan.UFC.Users.WebAPI.Entities;

namespace Titan.UFC.Users.WebAPI.Mappers
{
    public class UserAddressMap
    {
        public UserAddressMap(EntityTypeBuilder<UserAddressEntity> entityBuilder)
        {
            entityBuilder.HasKey(t => new { t.UserID, t.AddressID });
            entityBuilder.HasOne<UserEntity>(u => u.User).WithMany(ua => ua.UserAddresses).HasForeignKey(ua => ua.UserID).OnDelete(DeleteBehavior.Cascade);
            entityBuilder.HasOne<AddressEntity>(a => a.Address).WithOne(ua => ua.UserAddress).HasForeignKey<UserAddressEntity>(a => a.AddressID).OnDelete(DeleteBehavior.Cascade);
            entityBuilder.Property(t => t.CreatedDate);
        }
    }
}
