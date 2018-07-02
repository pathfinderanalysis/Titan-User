using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Entities;

namespace Titan.UFC.Users.WebAPI.Mappers
{
    public class AddressMap
    {
        public AddressMap(EntityTypeBuilder<AddressEntity> entityBuilder)
        {
            entityBuilder.HasKey(t => t.AddressID);
            entityBuilder.Property(t => t.Address1);
            entityBuilder.Property(t => t.Address2);
            entityBuilder.Property(t => t.City);
            entityBuilder.Property(t => t.CountryCode);
            entityBuilder.Property(t => t.StateID);
            entityBuilder.Property(t => t.PinCode);
            entityBuilder.Property(t => t.CreatedDate);
            entityBuilder.Property(t => t.UpdatedDate);
        }
    }
}
