using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Titan.UFC.Users.WebAPI.Entities;
using Titan.UFC.Users.WebAPI.Mappers;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Repositories;
using Xunit;

namespace Titan.UFC.Users.WebAPI.Tests.Repository
{
    public class UserRepositoryTest
    {
        protected UserRepository UserRepositoryUnderTest { get; }
        protected ApplicationDbContext DbContextInMemory { get; }
        protected MapperConfiguration MappingConfig { get; }
        protected IMapper Mapper { get; }

        protected Mock<ILogger<UserRepository>> LoggerMock { get; }

        public UserRepositoryTest()
        {
            DbContextInMemory = GetInMemoryDbContext();
            MappingConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingsProfile()); });
            Mapper = MappingConfig.CreateMapper();
            LoggerMock = new Mock<ILogger<UserRepository>>();
            UserRepositoryUnderTest = new UserRepository(DbContextInMemory, Mapper, LoggerMock.Object);            
        }

        public class ReadOneAsync : UserRepositoryTest
        {
            [Fact]
            public async void Should_return_User_when_exist()
            {
                // Arrange
                var userId = 1;
                var userEntity = UserDataEntity;

                //Act
                var result = await UserRepositoryUnderTest.ReadOneAsync(userId);

                //Assert
                Assert.Equal(userEntity.UserID, result.UserID);
                Assert.Equal(userEntity.AuthRefID, result.AuthRefID);
            }
        }

        public class CreateAsync : UserRepositoryTest
        {
            [Fact]
            public async void Should_return_nullexception_when_user_is_null()
            {
                // Arrange
                User user = null;
                var DbContextInMemory = GetInMemoryDbContext();
                var UserEntityInMemory = DbContextInMemory.Set<UserEntity>();
                var LoggerMock = new Mock<ILogger<UserRepository>>();
                var UserRepositoryUnderTest = new UserRepository(DbContextInMemory, Mapper, LoggerMock.Object);
                
                //Act
                var result = UserRepositoryUnderTest.CreateAsync(user);

                //Assert
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
            }

            [Fact]
            public async void Should_return_object_when_user_created()
            {

                var DbContextInMemory = GetInMemoryDbContext();
                var UserEntityInMemory = DbContextInMemory.Set<UserEntity>();
                var LoggerMock = new Mock<ILogger<UserRepository>>();
                var UserRepositoryUnderTest = new UserRepository(DbContextInMemory, Mapper, LoggerMock.Object);
                User user = UserData;
                user.UserID = 10;
                user.PhotoUri = "http://photouri/23298873";
                user.AuthRefID = new Guid("cfd5935c-7a60-40ea-8a27-e7452e50fa5f");
                var resultObject = new
                {
                    Id = 10,
                    AuthRefId = "cfd5935c-7a60-40ea-8a27-e7452e50fa5f",
                    PhotoUri = "http://photouri/23298873"
                };

                //Act
                var result = await UserRepositoryUnderTest.CreateAsync(user);

                //Assert

                Assert.Equal(resultObject.Id, GetValue("Id"));
                Assert.Equal(new Guid(resultObject.AuthRefId), GetValue("AuthRefId"));
                Assert.Equal(resultObject.PhotoUri, GetValue("PhotoUri"));

                object GetValue(string Name)
                {

                    return result.GetType().GetProperty(Name).GetValue(result);
                }
            }
        }

        public class DeleteAsync : UserRepositoryTest
        {
            [Fact]
            public async void Should_delete_when_user_exist()
            {
                // Arrange
                var ExistingId = 1;
                var DbContextInMemory = GetInMemoryDbContext();
                var UserEntityInMemory = DbContextInMemory.Set<UserEntity>();
                var LoggerMock = new Mock<ILogger<UserRepository>>();
                var UserRepositoryUnderTest = new UserRepository(DbContextInMemory, Mapper, LoggerMock.Object);

                // Act 
                await UserRepositoryUnderTest.DeleteAsync(ExistingId);

                // Assert
                Assert.Null(DbContextInMemory.Find<UserEntity>(ExistingId));
            }            
        }

        public class IsExistAsync : UserRepositoryTest
        {
            [Fact]
            public async void Should_return_true_when_user_exist()
            {
                // Arrange
                var DbContextInMemory = GetInMemoryDbContext();
                var UserEntityInMemory = DbContextInMemory.Set<UserEntity>();
                var LoggerMock = new Mock<ILogger<UserRepository>>();
                var UserRepositoryUnderTest = new UserRepository(DbContextInMemory, Mapper, LoggerMock.Object);
                var existingId = 1;

                // Act
                var result = await UserRepositoryUnderTest.IsExistAsync(existingId);

                // Assert
                Assert.True(result == true);
            }

            [Fact]
            public async void Should_return_false_when_user_unexist()
            {
                // Arrange
                var DbContextInMemory = GetInMemoryDbContext();
                var UserEntityInMemory = DbContextInMemory.Set<UserEntity>();
                var LoggerMock = new Mock<ILogger<UserRepository>>();
                var UserRepositoryUnderTest = new UserRepository(DbContextInMemory, Mapper, LoggerMock.Object);
                var unExistingId = 123;

                // Act
                var result = await UserRepositoryUnderTest.IsExistAsync(unExistingId);

                // Assert
                Assert.False(result == true);
            }
        }

        private static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            var context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            UserEntity userEntity = UserDataEntity;

            context.Add<UserEntity>(userEntity);
            context.SaveChanges();

            return context;
        }

        protected static UserEntity UserDataEntity => new UserEntity()
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
                },
            UserPhone = new UserPhoneEntity()
            {
                PhoneNumber = "9876543210",
                CallingCode = "123",
                IsVerified = false
            }
        };

        protected User UserData => new User()
        {
            //UserID=0,
            FirstName = "Richard Son",
            LastName = "Redding",
            Locale = "M",
            MiddleName = "J",
            Password = "tester123",
            TimeFormat = "HH",
            TimeZone = "GMT",
            Addresses = new List<Address>()
                        {
                          new Address()
                            {
                               Address1="1601 Hood Avenue",
                                Address2 ="dolore in adipisicing et",
                                City ="San Diego",
                                CountryCode ="11",
                                PinCode ="92103",
                                StateId =1
                            },
                           new Address()
                            {
                               Address1="3427 Allison Avenue",
                                Address2 ="Houston",
                                City ="Virginia Beach",
                                CountryCode ="12",
                                PinCode ="23462",
                                StateId =3
                           }, new Address()
                            {

                                Address1="1342 Baker Avenue",
                                Address2 ="Roanoke",
                                City ="Texas",
                                CountryCode ="45",
                                PinCode ="76262",
                                StateId =1
                            }

                        },
            Contacts = new Contacts
            {
                Phone =
                new Phone()
                {
                    Number = "9876543210",
                    CountryCode = "123"
                }
            }
        };

    }
}
