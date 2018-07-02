using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Titan.UFC.Users.WebAPI.Entities;
using Titan.UFC.Users.WebAPI.Mappers;
using Titan.UFC.Users.WebAPI.Repositories;
using Xunit;

namespace Titan.UFC.Users.WebAPI.Tests.Repository
{
   public class EmailRepositoryTest
    {


        protected EmailRepository EmailRepositoryUnderTest { get; }
        protected ApplicationDbContext DbContextInMemory { get; }
        protected MapperConfiguration MappingConfig { get; }
        protected IMapper Mapper { get; }

        protected Mock<ILogger<EmailRepository>> LoggerMock { get; }

        public EmailRepositoryTest()
        {
            DbContextInMemory = GetInMemoryDbContext();
            MappingConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingsProfile()); });
            Mapper = MappingConfig.CreateMapper();
            LoggerMock = new Mock<ILogger<EmailRepository>>();
            EmailRepositoryUnderTest = new EmailRepository(DbContextInMemory, Mapper, LoggerMock.Object);
        }


        private static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            var context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            UserEmailEntity userEmailEntity = UserEmailEntityData;

            context.Add<UserEmailEntity>(userEmailEntity);
            context.SaveChanges();

            return context;
        }


        public class ReadOneEmailAsync : EmailRepositoryTest
        {
            [Fact]
            public async void Should_return_Email_when_exist()
            {
                // Arrange
                var userId = 1;
                var userEmailEntity = UserEmailEntityData;

                //Act
                var result = await EmailRepositoryUnderTest.ReadOneAsync(userId);

                //Assert
                Assert.Equal(userEmailEntity.UserEmailID, result.Id);
                Assert.Equal(userEmailEntity.EmailAddress, result.Address);
            }

        }

        public class DeleteEmailAsync : EmailRepositoryTest
        {

            [Fact]
            public async void Should_delete_when_email_exist()
            {
                // Arrange
                var ExistingId = 1;
                var DbContextInMemory = GetInMemoryDbContext();
                var EmailEntityInMemory = DbContextInMemory.Set<UserEmailEntity>();
                var LoggerMock = new Mock<ILogger<EmailRepository>>();
                var EmailRepositoryUnderTest = new EmailRepository(DbContextInMemory, Mapper, LoggerMock.Object);

                // Act 
                await EmailRepositoryUnderTest.RemoveAsync(ExistingId);

                // Assert
                Assert.Null(DbContextInMemory.Find<UserEmailEntity>(ExistingId));
            }
        }

        protected static UserEmailEntity UserEmailEntityData => new UserEmailEntity()
        {
            EmailAddress="titan@honeywell.com",
            IsVerified = false,
            UserID = 1,
            UserEmailID = 2,
            VerificationKey = null,
            VerifiedDate = DateTime.UtcNow,
            User = new UserEntity()
            {
                UserID = 1,
                AuthRefID = new Guid("cfd5935c-7a60-40ea-8a27-e7452e50fa5f"),
                FirstName = "Richard Son",
                LastName = "Redding",
                Locale = "M",
                MiddleName = "J",
                TimeFormat = "HH",
                TimeZone = "GMT",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                DateFormat = "dd-MM-yyyy",
                IsEnabled = true,
                LastLoginTimeStamp = DateTime.Now,
                PhotoUri = "http://photouri/1290912",
                UserAddresses = new List<UserAddressEntity>()
                {
                    new UserAddressEntity()
                    {
                        AddressID = 1,
                        Address = new AddressEntity()
                        {
                            AddressID = 1,
                            Address1="1601 Hood Avenue",
                            Address2 ="dolore in adipisicing et",
                            City ="San Diego",
                            CountryCode ="11",
                            PinCode ="92103",
                            StateID =1,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsVerified = false,
                        },
                        CreatedDate = DateTime.Now,
                        UserID = 1,
                        User = new UserEntity()
                    }
                }
            }

        };
    }
}
