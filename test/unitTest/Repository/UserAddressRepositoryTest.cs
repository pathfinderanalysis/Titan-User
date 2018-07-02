using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Titan.UFC.Users.WebAPI.Entities;
using Titan.UFC.Users.WebAPI.Mappers;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Repositories;
using Xunit;

namespace Titan.UFC.Users.WebAPI.Tests.Repository
{
    public class UserAddressRepositoryTest
    {
        protected UserAddressRepository UserAddressRepositoryUnderTest { get; }
        protected ApplicationDbContext DbContextInMemory { get; }
        protected MapperConfiguration MappingConfig { get; }
        protected IMapper Mapper { get; }

        protected Mock<ILogger<UserAddressRepository>> LoggerMock { get; }

        public UserAddressRepositoryTest()
        {
            DbContextInMemory = GetInMemoryDbContext();
            MappingConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingsProfile()); });
            Mapper = MappingConfig.CreateMapper();
            LoggerMock = new Mock<ILogger<UserAddressRepository>>();
            UserAddressRepositoryUnderTest = new UserAddressRepository(DbContextInMemory, Mapper, LoggerMock.Object);
        }

        private static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            var context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            UserAddressEntity userAddressEntity = UserAddressDataEntity;

            context.Add<UserAddressEntity>(userAddressEntity);
            context.SaveChanges();

            return context;
        }

        public class CreateAsync : UserAddressRepositoryTest
        {
            [Fact]
            public async void Should_return_nullexception_when_address_is_null()
            {
                // Arrange
                Address address = null;
                int userId = 1;
                var DbContextInMemory = GetInMemoryDbContext();
                var LoggerMock = new Mock<ILogger<UserAddressRepository>>();
                var UserAddressRepositoryUnderTest = new UserAddressRepository(DbContextInMemory, Mapper, LoggerMock.Object);

                //Act
                var result = UserAddressRepositoryUnderTest.CreateAsync(userId, address);

                //Assert
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
            }

            [Fact]
            public async void Should_return_object_when_address_created()
            {
                int userId = 1;
                var DbContextInMemory = GetInMemoryDbContext();
                var LoggerMock = new Mock<ILogger<UserAddressRepository>>();
                var UserAddressRepositoryUnderTest = new UserAddressRepository(DbContextInMemory, Mapper, LoggerMock.Object);
                Address address = UserDataEntity;
                var resultObject = new Address
                {
                    Address1 = "1601 Hood Avenue",
                    Address2 = "dolore in adipisicing et",
                    City = "San Diego",
                    CountryCode = "11",
                    PinCode = "92103",
                    StateId = 1,
                    IsVerified = false
                };

                //Act
                var result = await UserAddressRepositoryUnderTest.CreateAsync(userId, address);

                //Assert
                Assert.Equal(resultObject.Address1, GetValue("Address1"));
                Assert.Equal(resultObject.Address2, GetValue("Address2"));
                Assert.Equal(resultObject.City, GetValue("City"));
                Assert.Equal(resultObject.CountryCode, GetValue("CountryCode"));
                Assert.Equal(resultObject.PinCode, GetValue("PinCode"));
                Assert.Equal(resultObject.StateId, GetValue("StateId"));
                Assert.Equal(resultObject.IsVerified, GetValue("IsVerified"));


                object GetValue(string Name)
                {

                    return result.GetType().GetProperty(Name).GetValue(result);
                }
            }
        }

        public class ReadAsync : UserAddressRepositoryTest
        {
            [Fact]
            public async void Should_return_Address_when_userId_exist()
            {
                // Arrange
                var userId = 1;
                var userEntity = UserAddressDataEntity;
                var DbContextInMemory = GetInMemoryDbContext();
                var LoggerMock = new Mock<ILogger<UserAddressRepository>>();
                var UserAddressRepositoryUnderTest = new UserAddressRepository(DbContextInMemory, Mapper, LoggerMock.Object);

                //Act
                var result = await UserAddressRepositoryUnderTest.ReadAsync(userId);

                //Assert
                Assert.Equal(userEntity.Address.Address1, GetValue("Address1"));
                Assert.Equal(userEntity.Address.Address2, GetValue("Address2"));

                object GetValue(string Name)
                {

                    return result[0].GetType().GetProperty(Name).GetValue(result[0]);
                }
            }

        }

        public class ReadUserAddress : UserAddressRepositoryTest
        {
            [Fact]
            public async void Should_return_Address_when_userId_exist()
            {
                // Arrange
                var userId = 1;
                var addressId = 99;
                var userEntity = UserAddressDataEntity;
                var DbContextInMemory = GetInMemoryDbContext();
                var LoggerMock = new Mock<ILogger<UserAddressRepository>>();
                var UserAddressRepositoryUnderTest = new UserAddressRepository(DbContextInMemory, Mapper, LoggerMock.Object);

                //Act
                var result = await UserAddressRepositoryUnderTest.ReadOneAsync(userId,addressId);

                //Assert
                Assert.Equal(userEntity.Address.Address1, GetValue("Address1"));
                Assert.Equal(userEntity.Address.Address2, GetValue("Address2"));

                object GetValue(string Name)
                {

                    return result.GetType().GetProperty(Name).GetValue(result);
                }
            }

        }

        protected static Address UserDataEntity = new Address()
        {
            AddressID = 1,
            Address1 = "1601 Hood Avenue",
            Address2 = "dolore in adipisicing et",
            City = "San Diego",
            CountryCode = "11",
            PinCode = "92103",
            StateId = 1,
            IsVerified = false,
        };

        protected static UserAddressEntity UserAddressDataEntity = new UserAddressEntity()
        {
            Address = new AddressEntity
            {
                AddressID = 99,
                Address1 = "1601 Hood Avenue",
                Address2 = "dolore in adipisicing et",
                City = "San Diego",
                CountryCode = "11",
                PinCode = "92103",
                StateID = 1,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsVerified = false,
            },
            UserID = 1,
            CreatedDate = DateTime.UtcNow
        };

}
}
