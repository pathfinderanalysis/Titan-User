using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Titan.UFC.Users.WebAPI.Controllers;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Resources;
using Titan.UFC.Users.WebAPI.Services;
using Xunit;
namespace Titan.UFC.Users.WebAPI.Tests.Controllers
{
   public class MeControllerTest
    {

        protected MeController ControllerUnderTest { get; }
        protected Mock<IUserService> UserServiceMock { get; }
        protected Mock<ILogger<UserController>> LoggerMock { get; }
        protected Mock<ISharedResource> LocalizerMock { get; }
        protected Mock<IConfiguration> Configuration { get; }

        public MeControllerTest()
        {
            UserServiceMock = new Mock<IUserService>();
            LoggerMock = new Mock<ILogger<UserController>>();
            LocalizerMock = new Mock<ISharedResource>();
            Configuration = new Mock<IConfiguration>();
            ControllerUnderTest = new MeController(UserServiceMock.Object,LoggerMock.Object, LocalizerMock.Object);
            ControllerUnderTest.ControllerContext = new ControllerContext();
            ControllerUnderTest.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        public class ReadOneAsync : MeControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_a_User()
            {
                // Arrange
                var user = Data;
                UserServiceMock
                    .Setup(x => x.ReadOneAsync(1))
                    .ReturnsAsync(user);

                ControllerUnderTest.ControllerContext.HttpContext.Request.Headers.Add("id", "1");

                // Act
                var result = await ControllerUnderTest.GetUser();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(user, okResult.Value);
            }

            [Fact]
            public async void Should_return_NotFoundResult_when_UserNotFoundException_is_thrown()
            {
                // Arrange
                var unexistingUserId = 0;
                string key = "Me_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                UserServiceMock
                    .Setup(x => x.ReadOneAsync(unexistingUserId))
                    .ThrowsAsync(new Exception(unexistingUserId.ToString()));

                LocalizerMock
                    .Setup(_ => _[key]).Returns(localizedString);

                // set header values

                ControllerUnderTest.ControllerContext.HttpContext.Request.Headers.Add("id", unexistingUserId.ToString());

                // ActA
                var result = await ControllerUnderTest.GetUser();

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }
        }

        public class UpdateAsync : MeControllerTest
        {

            [Fact]
            public async void Should_return_NoFoundResult_when_Id_is_invalid()
            {
                //Arrange
                var unexistingUserId = 2;
                var user = UserUpdate;
                var key = "Me_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                // set header values

                ControllerUnderTest.ControllerContext.HttpContext.Request.Headers.Add("id", unexistingUserId.ToString());

                //Act
                var result = await ControllerUnderTest.Update(user);

                //Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }

            [Fact]
            public async void Should_return_BadRequest_when_model_is_null()
            {
                //Arrange
                UserUpdate user = null;
                var userId = 1;
                var returnExistValue = true;
                var key = "Me_StatusCode_400_BadRequest";
                var localizedString = new LocalizedString(key, "Bad Request");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                // Service
                UserServiceMock.Setup(x => x.IsExistAsync(userId)).ReturnsAsync(returnExistValue);

                // set header values
                ControllerUnderTest.ControllerContext.HttpContext.Request.Headers.Add("id", userId.ToString());

                //Act 
                var result = await ControllerUnderTest.Update(user);

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }

            [Fact]
            public async void Should_return_OkResult_when_model_is_valid()
            {
                //Arrange
                var userId = 1;
                var user = UserUpdate;
                var returnExistValue = true;
                // Service
                UserServiceMock.Setup(x => x.IsExistAsync(userId)).ReturnsAsync(returnExistValue);
                // set header values
                ControllerUnderTest.ControllerContext.HttpContext.Request.Headers.Add("id", userId.ToString());
                //Act 
                var result = await ControllerUnderTest.Update(user);

                //Assert
                Assert.IsType<OkResult>(result);
            }
        }

        public class DeleteAsync : MeControllerTest
        {
            [Fact]
            public async void Should_return_NotFoundResult_when_id_is_invalid()
            {
                // Arrange
                var unexistingUserId = 2;
                var key = "Me_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                LocalizerMock
                .Setup(_ => _[key]).Returns(localizedString);

                UserServiceMock
                   .Setup(o => o.RemoveAsync(unexistingUserId))
                   .ThrowsAsync(new ArgumentException(unexistingUserId.ToString()));

                ControllerUnderTest.ControllerContext.HttpContext.Request.Headers.Add("id", unexistingUserId.ToString());

                // Act
                var result = await ControllerUnderTest.Delete();

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }

            [Fact]
            public async void Should_return_NoContentResult_when_Id_is_valid()
            {
                // Arrange
                var userId = 1;
                // var returnResultObject = true;
                var returnExpectedStatusCode = 204;

                ControllerUnderTest.ControllerContext.HttpContext.Request.Headers.Add("id", userId.ToString());

                //Act 
                var result = await ControllerUnderTest.Delete();

                //Assert
                Assert.IsType<NoContentResult>(result);

                var NoContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(returnExpectedStatusCode, NoContentResult.StatusCode);
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
            UserID = 1,
            FirstName = "Richard Son new",
            LastName = "Redding new",
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
