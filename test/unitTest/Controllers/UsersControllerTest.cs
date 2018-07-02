using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Titan.UFC.Users.WebAPI.Controllers;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Resources;
using Titan.UFC.Users.WebAPI.Services;
using Xunit;

namespace Titan.UFC.Users.WebAPI.Tests.Controllers
{
    public class UsersControllerTest
    {
        protected UserController ControllerUnderTest { get; }
        protected Mock<IUserService> UserServiceMock { get; }
        protected Mock<ILogger<UserController>> LoggerMock { get; }
        protected Mock<ISharedResource> LocalizerMock { get; }
        protected Mock<IConfiguration> ConfigurationMock { get; }
        protected Mock<IContactsService> ContactServiceMock { get; }

        public UsersControllerTest()
        {
            UserServiceMock = new Mock<IUserService>();
            ContactServiceMock = new Mock<IContactsService>();
            LoggerMock = new Mock<ILogger<UserController>>();
            LocalizerMock = new Mock<ISharedResource>();
            ConfigurationMock = new Mock<IConfiguration>();
            ControllerUnderTest = new UserController(UserServiceMock.Object, ContactServiceMock.Object, LoggerMock.Object, LocalizerMock.Object, ConfigurationMock.Object);
        }

        public class ReadOneAsync : UsersControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_a_User()
            {
                // Arrange
                var user = Data;
                UserServiceMock
                    .Setup(x => x.ReadOneAsync(1))
                    .ReturnsAsync(user);

                // Act
                var result = await ControllerUnderTest.GetUser(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(user, okResult.Value);
            }

            [Fact]
            public async void Should_return_NotFoundResult_when_UserNotFoundException_is_thrown()
            {
                // Arrange
                var unexistingUserId = 2;
                string key = "User_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                UserServiceMock
                    .Setup(x => x.ReadOneAsync(unexistingUserId))
                    .ThrowsAsync(new Exception(unexistingUserId.ToString()));

                LocalizerMock
                    .Setup(_ => _[key]).Returns(localizedString);

                // Act
                var result = await ControllerUnderTest.GetUser(unexistingUserId);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }
        }

        public class CreateAsync : UsersControllerTest
        {
            [Fact]
            public async void Should_return_BadRequest_when_model_is_null()
            {
                // Arrange
                var key = "User_StatusCode_400_BadRequest";
                var localizedString = new LocalizedString(key, "Bad Request");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                // Act
                var result = await ControllerUnderTest.CreateUser(null);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }

            [Fact]
            public async void Should_return_CreatedResult_when_model_Is_valid()
            {
                // Arrange
                var user = Data;
                var resultObject = new
                {
                    Id = 1,
                    AuthRefId = "cfd5935c-7a60-40ea-8a27-e7452e50fa5f",
                    PhotoUri = "http://photouri/23298873"
                };

                UserServiceMock
                    .Setup(x => x.CreateAsync(user))
                    .ReturnsAsync(resultObject);

                //Act 
                var result = await ControllerUnderTest.CreateUser(user);

                //Assert
                var okResult = Assert.IsType<CreatedResult>(result);
                Assert.Same(resultObject, okResult.Value);
            }
        }

        public class UpdateAsync : UsersControllerTest
        {
            [Fact]
            public async void Should_return_BadRequestResult_when_Id_is_zero()
            {
                //Arrange
                var unexistingUserId = 0;
                var userData = UserUpdate;
                var key = "User_StatusCode_400_BadRequest";
                var localizedString = new LocalizedString(key, "Bad Request");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                //Act
                var result = await ControllerUnderTest.Update(unexistingUserId, userData);

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }

            [Fact]
            public async void Should_return_NoFoundResult_when_Id_is_invalid()
            {
                //Arrange
                var unexistingUserId = 2;
                var user = UserUpdate;
                var key = "User_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                //Act
                var result = await ControllerUnderTest.Update(unexistingUserId, user);

                //Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }

            [Fact]
            public async void Should_return_OkResult_when_model_is_null()
            {
                //Arrange
                UserUpdate user = null;
                var userId = 6;
                var returnExistValue = true;
                var key = "User_StatusCode_400_BadRequest";
                var localizedString = new LocalizedString(key, "Bad Request");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                // Service
                UserServiceMock.Setup(x => x.IsExistAsync(userId)).ReturnsAsync(returnExistValue);

                //Act 
                var result = await ControllerUnderTest.Update(userId, user);

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }

            [Fact]
            public async void Should_return_OkResult_when_model_is_valid()
            {
                //Arrange
                var userId = 6;
                var user = UserUpdate;
                var returnExistValue = true;

                // Service
                UserServiceMock.Setup(x => x.IsExistAsync(userId)).ReturnsAsync(returnExistValue);

                //Act 
                var result = await ControllerUnderTest.Update(userId, user);

                //Assert
                Assert.IsType<OkResult>(result);
            }
        }

        public class DeleteAsync : UsersControllerTest
        {
            [Fact]
            public async void Should_return_NotFoundResult_when_id_is_invalid()
            {
                // Arrange
                var unexistingUserId = 2;
                var key = "User_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                LocalizerMock
                .Setup(_ => _[key]).Returns(localizedString);

                UserServiceMock
                   .Setup(o => o.RemoveAsync(unexistingUserId))
                   .ThrowsAsync(new ArgumentException(unexistingUserId.ToString()));

                // Act
                var result = await ControllerUnderTest.Delete(unexistingUserId);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }

            [Fact]
            public async void Should_return_NoContentResult_when_Id_is_valid()
            {
                // Arrange
                var userId = 6;
                // var returnResultObject = true;
                var returnExpectedStatusCode = 204;
                // Service
                // UserServiceMock.Setup(x => x.IsExistAsync(userId)).ReturnsAsync(returnResultObject);

                //Act 
                var result = await ControllerUnderTest.Delete(userId);

                //Assert
                Assert.IsType<NoContentResult>(result);

                var NoContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(returnExpectedStatusCode, NoContentResult.StatusCode);
            }
        }


        public class ForgotPasswordAsync : UsersControllerTest
        {

            [Fact]
            public async void Should_return_createdObjectResult_when_phone_is_valid()
            {
                // Arrange
                var validUserId = 1;
                var expirationDays = "2";
                Contacts contact = new Contacts()
                {
                    Phone = new Phone
                    {
                        CountryCode = "123",
                        Number = "7894561230"
                    }
                };

                ContactServiceMock.Setup(x => x.CheckValidUserByPhoneAsync(contact.Phone))
                    .ReturnsAsync(validUserId);

                ConfigurationMock.Setup(x => x["PasswordConfig:OTPExpiryTime"]).Returns(expirationDays);

                ForgotPassword forgotPassword = new ForgotPassword()
                {
                    UserID = validUserId,
                    OTPExpiryTime = DateTime.UtcNow.AddDays(Convert.ToInt16(expirationDays)),
                    ResetOTP = ControllerUnderTest.GetOTP(),
                    ResetTime = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow

                };

                var expectedResult = forgotPassword;

                UserServiceMock.Setup(y => y.CreateAsync(forgotPassword))
                                .ReturnsAsync(expectedResult);

                // Act
                var result = await ControllerUnderTest.ForgotPassword(contact);

                // Assert 
                Assert.IsType<CreatedResult>(result);

            }

            [Fact]
            public async void Should_return_NotFound_when_phone_is_Invalid()
            {

                // Arrange
                var invalidUserId = 0;
                var expirationDays = "2";
                string key = "User_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");
                Contacts contact = new Contacts()
                {
                    Phone = new Phone
                    {
                        CountryCode = "123",
                        Number = "7894561230"
                    }
                };

                ContactServiceMock.Setup(x => x.CheckValidUserByPhoneAsync(contact.Phone))
                    .ReturnsAsync(invalidUserId);

                ConfigurationMock.Setup(x => x["PasswordConfig:OTPExpiryTime"])
                    .Returns(expirationDays);

                LocalizerMock
                    .Setup(_ => _[key]).Returns(localizedString);

                // Act 
                var result = await ControllerUnderTest.ForgotPassword(contact);

                Assert.IsType<NotFoundObjectResult>(result);

            }
            [Fact]
            public async void Should_return_BadRequest_when_contact_is_null()
            {
                // Arrange
                Contacts contact = new Contacts() { Phone = null };
                string key = "User_StatusCode_400_BadRequest";
                var localizedString = new LocalizedString(key, "Bad Request");

                LocalizerMock
                   .Setup(_ => _[key]).Returns(localizedString);

                // Assert 
                var result = await ControllerUnderTest.ForgotPassword(contact);

                Assert.IsType<BadRequestObjectResult>(result);

            }

        }

        protected static User Data => new User()
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

        protected static UserUpdate UserUpdate => new UserUpdate()
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
