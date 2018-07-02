using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Titan.UFC.Users.WebAPI.Entities;
using Titan.UFC.Users.WebAPI.Mappers;
using Titan.UFC.Users.WebAPI.Repositories;
using Xunit;

namespace Titan.UFC.Users.WebAPI.Tests.Repository
{
    public class PhoneRepositoryTest
    {
        protected PhoneRepository PhoneRepositoryUnderTest { get; }
        protected ApplicationDbContext DbContextInMemory { get; }
        protected MapperConfiguration MappingConfig { get; }
        protected IMapper Mapper { get; }

        protected Mock<ILogger<PhoneRepository>> LoggerMock { get; }

        public PhoneRepositoryTest()
        {
            DbContextInMemory = GetInMemoryDbContext();
            MappingConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingsProfile()); });
            Mapper = MappingConfig.CreateMapper();
            LoggerMock = new Mock<ILogger<PhoneRepository>>();
            PhoneRepositoryUnderTest = new PhoneRepository(DbContextInMemory, Mapper, LoggerMock.Object);
        }


        private static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            var context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            UserPhoneEntity userPhoneEntity = UserPhoneEntityData;

            context.Add<UserPhoneEntity>(userPhoneEntity);
            context.SaveChanges();

            return context;
        }


        public class ReadOnePhoneAsync : PhoneRepositoryTest
        {
            [Fact]
            public async void Should_return_Phone_when_exist()
            {
                // Arrange
                var userId = 1;
                var userPhoneEntity = UserPhoneEntityData;

                //Act
                var result = await PhoneRepositoryUnderTest.ReadOneAsync(userId);

                //Assert
                Assert.Equal(userPhoneEntity.UserPhoneID, result.Id);
                Assert.Equal(userPhoneEntity.PhoneNumber, result.Number);
            }

        }

        public class DeletePhoneAsync : PhoneRepositoryTest
        {

            [Fact]
            public async void Should_delete_when_phone_exist()
            {
                // Arrange
                var ExistingId = 1;
                var DbContextInMemory = GetInMemoryDbContext();
                var PhoneEntityInMemory = DbContextInMemory.Set<UserPhoneEntity>();
                var LoggerMock = new Mock<ILogger<PhoneRepository>>();
                var PhoneRepositoryUnderTest = new PhoneRepository(DbContextInMemory, Mapper, LoggerMock.Object);

                // Act 
                await PhoneRepositoryUnderTest.RemoveAsync(ExistingId);

                // Assert
                Assert.Null(DbContextInMemory.Find<UserPhoneEntity>(ExistingId));
            }
        }

        protected static UserPhoneEntity UserPhoneEntityData => new UserPhoneEntity()
        {
            CallingCode = "123",
            IsVerified = false,
            PhoneNumber = "7894561230",
            UserID = 1,
            UserPhoneID = 2,
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
