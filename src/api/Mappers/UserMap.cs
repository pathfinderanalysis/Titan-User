using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Titan.UFC.Users.WebAPI.Entities;

namespace Titan.UFC.Users.WebAPI.Mappers
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<UserEntity> entityBuilder)
        {
            entityBuilder.HasKey(t => t.UserID);
            entityBuilder.Property(t => t.AuthRefID).IsRequired();
            entityBuilder.Property(t => t.FirstName);
            entityBuilder.Property(t => t.MiddleName);
            entityBuilder.Property(t => t.LastName);
            entityBuilder.Property(t => t.IsEnabled).IsRequired();
            entityBuilder.Property(t => t.CreatedDate).IsRequired();
            entityBuilder.Property(t => t.UpdatedDate);
            entityBuilder.Property(t => t.Locale);
            entityBuilder.Property(t => t.DateFormat);
            entityBuilder.Property(t => t.TimeFormat);
            entityBuilder.Property(t => t.TimeZone);
            entityBuilder.Property(t => t.PhotoUri);
            entityBuilder.Property(t => t.LastLoginTimeStamp);
        }
    }
}
