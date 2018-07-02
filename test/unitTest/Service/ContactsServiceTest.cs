using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Repositories;
using Titan.UFC.Users.WebAPI.Services;
using Xunit;

namespace Titan.UFC.Users.WebAPI.Tests.Service
{
    public class ContactsServiceTest
    {
        protected ContactsService ContactServiceUnderTest { get; }
        protected Mock<IEmailRepository> EmailRepositoryMock { get; }

        protected Mock<IPhoneRepository> PhoneRepositoryMock { get; }

        protected Mock<ILogger<ContactsService>> LoggerMock { get; }

        public ContactsServiceTest()
        {
            EmailRepositoryMock = new Mock<IEmailRepository>();
            PhoneRepositoryMock = new Mock<IPhoneRepository>();
            LoggerMock = new Mock<ILogger<ContactsService>>();
            ContactServiceUnderTest = new ContactsService(PhoneRepositoryMock.Object, EmailRepositoryMock.Object, LoggerMock.Object);
        }

        public class ReadOnePhoneAsync : ContactsServiceTest
        {
            [Fact]
            public void Should_return_Phone_when_user_Id_exist()
            {
                //Arrange
                var existingUserId = 6;
                var expectedPhone = PhoneData;

                PhoneRepositoryMock
                    .Setup(i => i.ReadOneAsync(existingUserId))
                    .ReturnsAsync(expectedPhone);

                //Act
                var result = ContactServiceUnderTest.ReadOnePhoneAsync(existingUserId);

                //Assert
                Assert.Same(expectedPhone, result.Result);
            }

            [Fact]
            public void Should_return_null_when_phone_notExist()
            {
                //Arrange
                var existingUserId = 6;
                Phone phone = null;

                PhoneRepositoryMock
                    .Setup(i => i.ReadOneAsync(existingUserId))
                    .ReturnsAsync(phone);

                //Act
                var result = ContactServiceUnderTest.ReadOnePhoneAsync(existingUserId);

                //Assert
                Assert.Null(result.Result);
            }

        }

        public class ReadOneEmailAsync : ContactsServiceTest
        {

            [Fact]
            public void Should_return_Email_when_user_Id_exist()
            {
                //Arrange
                var existingUserId = 6;
                var expectedEmail = EmailData;

                EmailRepositoryMock
                    .Setup(i => i.ReadOneAsync(existingUserId))
                    .ReturnsAsync(expectedEmail);

                //Act
                var result = ContactServiceUnderTest.ReadOneEmailAsync(existingUserId);

                //Assert
                Assert.Same(expectedEmail, result.Result);
            }

            [Fact]
            public void Should_return_null_when_email_notExist()
            {
                //Arrange
                var existingUserId = 6;
                Email email = null;

                EmailRepositoryMock
                    .Setup(i => i.ReadOneAsync(existingUserId))
                    .ReturnsAsync(email);

                //Act
                var result = ContactServiceUnderTest.ReadOneEmailAsync(existingUserId);

                //Assert
                Assert.Null(result.Result);
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
