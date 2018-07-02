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
    public class AddressServiceTest
    {
        protected AddressService AddressServiceUnderTest { get; }
        protected Mock<IUserAddressRepository> UserAddressRepositoryMock { get; }
        protected Mock<ILogger<AddressService>> LoggerMock { get; }

        public AddressServiceTest()
        {
            UserAddressRepositoryMock = new Mock<IUserAddressRepository>();
            LoggerMock = new Mock<ILogger<AddressService>>();
            AddressServiceUnderTest = new AddressService(UserAddressRepositoryMock.Object, LoggerMock.Object);
        }

        public class CreateAsync : AddressServiceTest
        {
            [Fact]
            public void Should_return_object_when_address_created()
            {
                //Arrange
                Address address = AddressDataEntity;
                int userId = 1;
                var resultObject = new Address()
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

                UserAddressRepositoryMock
                    .Setup(i => i.CreateAsync(userId,address))
                    .ReturnsAsync(resultObject);

                //Act
                var result = AddressServiceUnderTest.CreateAsync(userId,address);

                //Assert
                Assert.Same(resultObject, result.Result);
            }

            [Fact]
            public void Should_return_nullexception_when_address_is_null()
            {
                //Arrange
                Address address = null;
                int userId = 1;
                UserAddressRepositoryMock
                    .Setup(i => i.CreateAsync(1, address))
                    .ThrowsAsync(new ArgumentNullException("User is null"));

                //Act
                var result = AddressServiceUnderTest.CreateAsync(userId,address);

                //Assert               
                Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
            }          

        }

        public class UpdateAsync : AddressServiceTest
        {
            [Fact]
            public void Should_return_nullexception_when_address_is_null()
            {
                //Arrange
                 List<Address>address = null;
                var userId = 8;
                UserAddressRepositoryMock
                    .Setup(i => i.UpdateAsync(userId, address))
                    .ThrowsAsync(new ArgumentNullException("User is null"));

                //Act
                var result = AddressServiceUnderTest.UpdateAsync(userId, address);

                //Assert               
                Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
            }


        }

        public class ReadAsync : AddressServiceTest
        {
            [Fact]
            public void Should_return_address_when_user_id_valid()
            {
                //Arrange
                var userId = 1;
                List<Address> expectedResult = new List<Address>() { AddressDataEntity };
                UserAddressRepositoryMock.Setup(i=>i.ReadAsync(userId)).ReturnsAsync(expectedResult);

                //Act
                var result = AddressServiceUnderTest.ReadAsync(userId);

                // assert
                Assert.Same(expectedResult, result.Result);

            }

        }

        public class ReadUserAddress : AddressServiceTest
        {

            [Fact]
            public void Should_return_address_when_address_id_valid()
            {
                //Arrange
                var userId = 1;
                var addressId = 99;
                var expectedResult = AddressDataEntity;
                UserAddressRepositoryMock.Setup(i => i.ReadOneAsync(userId,addressId)).ReturnsAsync(expectedResult);

                //Act
                var result = AddressServiceUnderTest.ReadOneAsync(userId,addressId);

                // assert
                Assert.Same(expectedResult, result.Result);

            }

        }

        public class UpdateUserAddress : AddressServiceTest
        {

            [Fact]
            public void Should_return_nullexception_when_address_is_null()
            {
                //Arrange
               Address address = null;
                var userId = 8;
                var addressId = 99;
                UserAddressRepositoryMock
                    .Setup(i => i.UpdateAsync(userId, addressId, address))
                    .ThrowsAsync(new ArgumentNullException("User is null"));

                //Act
                var result = AddressServiceUnderTest.UpdateAsync(userId, addressId, address);

                //Assert               
                Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
            }
        }


        protected static Address AddressDataEntity = new Address()
        {
            AddressID = 1,
            Address1 = "1601 Hood Avenue",
            Address2 = "dolore in adipisicing et",
            City = "San Diego",
            CountryCode = "11",
            PinCode = "92103",
            StateId = 1,
            IsVerified = false
        };
        protected static List<Address> Addresses => new List<Address>()
        {
            new Address()
            {
                    Address1 = "1601 Hood Avenue",
                    Address2 = "dolore in adipisicing et",
                    City = "San Diego",
                    CountryCode = "11",
                    PinCode = "92103",
                    StateId = 1,
                    IsVerified=false

            }, new Address()
            {
                   Address1 = "104 S Cedar St",
                   Address2 = "Pauls Valley",
                   City = "OK",
                   StateId = 4,
                   PinCode =  "73075",
                   CountryCode =  "26",
                   IsVerified=false

            }

        };

    }
}
