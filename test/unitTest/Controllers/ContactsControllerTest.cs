using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Titan.UFC.Users.WebAPI.Controllers;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Resources;
using Titan.UFC.Users.WebAPI.Services;
using Xunit;

namespace Titan.UFC.Users.WebAPI.Tests.Controllers
{
    public class ContactsControllerTest
    {
        protected ContactsController ControllerUnderTest { get; }
        protected Mock<IContactsService> ContactsServiceMock { get; }
        protected Mock<ILogger<ContactsController>> LoggerMock { get; }
        protected Mock<ISharedResource> LocalizerMock { get; }

        public ContactsControllerTest()
        {
            ContactsServiceMock = new Mock<IContactsService>();
            LoggerMock = new Mock<ILogger<ContactsController>>();
            LocalizerMock = new Mock<ISharedResource>();
            ControllerUnderTest = new ContactsController(ContactsServiceMock.Object, LoggerMock.Object, LocalizerMock.Object);
        }


        public class ReadOnePhoneAsync : ContactsControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_a_Phone()
            {
                // Arrange
                var phone = PhoneData;
                var userId = 1;
                ContactsServiceMock
                    .Setup(x => x.ReadOnePhoneAsync(userId))
                    .ReturnsAsync(phone);

                // Act
                var result = await ControllerUnderTest.GetUserPhone(userId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(phone, okResult.Value);

            }

            [Fact]
            public async void Should_return_NotFoundResult_when_PhoneNotFoundException_is_thrown()
            {
                // Arrange
                var unexistingUserId = 2;
                string key = "Contact_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                ContactsServiceMock
                    .Setup(x => x.ReadOnePhoneAsync(unexistingUserId))
                    .ThrowsAsync(new Exception(unexistingUserId.ToString()));

                LocalizerMock
                    .Setup(_ => _[key]).Returns(localizedString);

                // Act
                var result = await ControllerUnderTest.GetUserPhone(unexistingUserId);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }

        }

        public class ReadOneEmailAsync : ContactsControllerTest
        {

            [Fact]
            public async void Should_return_OkObjectResult_with_a_Email()
            {
                // Arrange
                var email = EmailData;
                var userId = 1;
                ContactsServiceMock
                    .Setup(x => x.ReadOneEmailAsync(userId))
                    .ReturnsAsync(email);

                // Act
                var result = await ControllerUnderTest.GetUserEmail(userId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(email, okResult.Value);

            }

            [Fact]
            public async void Should_return_NotFoundResult_when_EmailNotFoundException_is_thrown()
            {
                // Arrange
                var unexistingUserId = 2;
                string key = "Contact_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                ContactsServiceMock
                    .Setup(x => x.ReadOneEmailAsync(unexistingUserId))
                    .ThrowsAsync(new Exception(unexistingUserId.ToString()));

                LocalizerMock
                    .Setup(_ => _[key]).Returns(localizedString);

                // Act
                var result = await ControllerUnderTest.GetUserEmail(unexistingUserId);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }

        }


        public class DeletePhoneAsync : ContactsControllerTest
        {
            [Fact]
            public async void Should_return_NotFoundResult_when_id_is_invalid()
            {
                // Arrange
                var unexistingUserId = 2;
                var key = "Contact_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                LocalizerMock
                .Setup(_ => _[key]).Returns(localizedString);

                ContactsServiceMock
                   .Setup(o => o.RemovePhoneAsync(unexistingUserId))
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
               
                //Act 
                var result = await ControllerUnderTest.Delete(userId);

                //Assert
                Assert.IsType<NoContentResult>(result);

                var NoContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(returnExpectedStatusCode, NoContentResult.StatusCode);
            }

        }

        public class DeleteEmailAsync : ContactsControllerTest
        {

            [Fact]
            public async void Should_return_NotFoundResult_when_id_is_invalid()
            {
                // Arrange
                var unexistingUserId = 2;
                var key = "Contact_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                LocalizerMock
                .Setup(_ => _[key]).Returns(localizedString);

                ContactsServiceMock
                   .Setup(o => o.RemoveEmailAsync(unexistingUserId))
                   .ThrowsAsync(new ArgumentException(unexistingUserId.ToString()));

                // Act
                var result = await ControllerUnderTest.DeleteUserEmail(unexistingUserId);

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

                //Act 
                var result = await ControllerUnderTest.DeleteUserEmail(userId);

                //Assert
                Assert.IsType<NoContentResult>(result);

                var NoContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(returnExpectedStatusCode, NoContentResult.StatusCode);
            }
        }

        protected static Phone PhoneData => new Phone()
        {
            CountryCode = "123",
            Id = 1,
            Number = "7894561230",
            IsVerified = false

        };

        protected static Email EmailData => new Email()
        {
            Address = "titan@honeywell.com",
            Id = 1,
            IsVerified = false

        };
    }
}
