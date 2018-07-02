using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Entities;
using Titan.UFC.Users.WebAPI.Mappers;

namespace Titan.UFC.Users.WebAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new AddressMap(modelBuilder.Entity<AddressEntity>());
            new UserMap(modelBuilder.Entity<UserEntity>());            
            new UserAddressMap(modelBuilder.Entity<UserAddressEntity>());
            new UserEmailMap(modelBuilder.Entity<UserEmailEntity>());
            new UserPhoneMap(modelBuilder.Entity<UserPhoneEntity>());
            new ForgotPasswordMap(modelBuilder.Entity<ForgotPasswordEntity>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
