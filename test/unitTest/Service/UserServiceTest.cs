using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Repositories;
using Titan.UFC.Users.WebAPI.Services;
using Xunit;

namespace Titan.UFC.Users.WebAPI.Tests.Service
{
    public class UserServiceTest
    {
        protected UserService UserServiceUnderTest { get; }
        protected Mock<IUserRepository> UserRepositoryMock { get; }
        protected Mock<ILogger<UserService>> LoggerMock { get; }

        protected Mock<IForgotPasswordRepository> ForgotPasswordRepositoryMock { get; }

        public UserServiceTest()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            LoggerMock = new Mock<ILogger<UserService>>();
            ForgotPasswordRepositoryMock = new Mock<IForgotPasswordRepository>();
            UserServiceUnderTest = new UserService(UserRepositoryMock.Object, ForgotPasswordRepositoryMock.Object, LoggerMock.Object);
        }

        public class ReadOneAsync : UserServiceTest
        {
            [Fact]
            public void Should_return_User_when_user_exist()
            {
                //Arrange
                var existingUserId = 6;
                var expectedUser = Data;

                UserRepositoryMock
                    .Setup(i => i.ReadOneAsync(existingUserId))
                    .ReturnsAsync(expectedUser);

                //Act
                var result = UserServiceUnderTest.ReadOneAsync(existingUserId);
                
                //Assert
                Assert.Same(expectedUser, result.Result);
            }

            [Fact]
            public void Should_return_null_when_user_notExist()
            {
                //Arrange
                var existingUserId = 6;
                User user = null;

                UserRepositoryMock
                    .Setup(i => i.ReadOneAsync(existingUserId))
                    .ReturnsAsync(user);

                //Act
                var result = UserServiceUnderTest.ReadOneAsync(existingUserId);

                //Assert
                Assert.Null(result.Result);
            }
        }

        public class CreateAsync : UserServiceTest
        {
            [Fact]
            public void Should_return_object_when_user_created()
            {
                //Arrange
                User user = Data;
                var resultObject = new
                {
                    Id = 1,
                    AuthRefId = "cfd5935c-7a60-40ea-8a27-e7452e50fa5f",
                    PhotoUri = "http://photouri/23298873"
                };

                UserRepositoryMock
                    .Setup(i => i.CreateAsync(user))
                    .ReturnsAsync(resultObject);

                //Act
                var result = UserServiceUnderTest.CreateAsync(user);

                //Assert
                Assert.Same(resultObject, result.Result);
            }

            [Fact]
            public void Should_return_nullexception_when_user_is_null()
            {
                //Arrange
                User user = null;

                UserRepositoryMock
                    .Setup(i => i.CreateAsync(user))
                    .ThrowsAsync(new ArgumentNullException("User is null"));

                //Act
                var result = UserServiceUnderTest.CreateAsync(user);

                //Assert               
                Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
            }
        }

        public class UpdateAsync : UserServiceTest
        {
            [Fact]
            public void Should_return_nullexception_when_user_is_null()
            {
                //Arrange
                UserUpdate UserUpdate = null;

                UserRepositoryMock
                    .Setup(i => i.UpdateAsync(UserUpdate))
                    .ThrowsAsync(new ArgumentNullException("User is null"));

                //Act
                var result = UserServiceUnderTest.UpdateAsync(UserUpdate);

                //Assert               
                Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
            }
        }

        public class IsExistAsync : UserServiceTest
        {
            [Fact]
            public void should_return_exist_when_id_is_valid()
            {
                // Arrange
                var expectedUserId = 2;
                var expectedResult = true;
                UserRepositoryMock
                    .Setup(i => i.IsExistAsync(expectedUserId)).ReturnsAsync(expectedResult);

                // Act
                var result = UserServiceUnderTest.IsExistAsync(expectedUserId);

                // Assert
                Assert.True(result.Result == true);
            }

            [Fact]
            public void should_return_notExist_when_id_not_Exist()
            {
                // Arrange
                var expectedUserId = 2;
                var expectedResult = false;
                UserRepositoryMock
                    .Setup(i => i.IsExistAsync(expectedUserId)).ReturnsAsync(expectedResult);

                // Act
                var result = UserServiceUnderTest.IsExistAsync(expectedUserId);

                // Assert
                Assert.False(result.Result == true);
            }
        }

        private static User Data => new User()
        {
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

        private static UserUpdate UserUpdate => new UserUpdate()
        {
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
