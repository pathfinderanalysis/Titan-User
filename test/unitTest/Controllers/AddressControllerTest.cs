using Microsoft.AspNetCore.Mvc;
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
    public class AddressControllerTest
    {
        protected AddressController ControllerUnderTest { get; }
        protected Mock<IAddressService> AddressServiceMock { get; }
        protected Mock<IUserService> UserServiceMock { get; }
        protected Mock<ILogger<AddressController>> LoggerMock { get; }
        protected Mock<ISharedResource> LocalizerMock { get; }

        public AddressControllerTest()
        {
            AddressServiceMock = new Mock<IAddressService>();
            UserServiceMock = new Mock<IUserService>();
            LoggerMock = new Mock<ILogger<AddressController>>();
            LocalizerMock = new Mock<ISharedResource>();
            ControllerUnderTest = new AddressController(AddressServiceMock.Object, UserServiceMock.Object, LoggerMock.Object, LocalizerMock.Object);
        }

        public class CreateAsync : AddressControllerTest
        {
            [Fact]
            public async void Should_return_BadRequest_when_model_is_null()
            {
                // Arrange
                var key = "Address_StatusCode_400_BadRequest";
                var localizedString = new LocalizedString(key, "Bad Request");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                // Act
                var result = await ControllerUnderTest.CreateAddress(1, null);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }

            [Fact]
            public async void Should_return_CreatedResult_when_model_Is_valid()
            {
                // Arrange
                var returnExistValue = true;
                var address = AddressData;
                int userId = 1;
                var resultObject = new Address
                {
                    Address1 = "1601 Hood Avenue",
                    Address2 = "dolore in adipisicing et",
                    City = "San Diego",
                    CountryCode = "11",
                    PinCode = "92103",
                    StateId = 1
                };

                AddressServiceMock
                    .Setup(x => x.CreateAsync(userId, address))
                    .ReturnsAsync(resultObject);

                UserServiceMock
                    .Setup(x => x.IsExistAsync(userId))
                    .ReturnsAsync(true);

                //Act 
                UserServiceMock.Setup(x => x.IsExistAsync(userId)).ReturnsAsync(returnExistValue);
                var result = await ControllerUnderTest.CreateAddress(userId, address);

                //Assert
                var okResult = Assert.IsType<CreatedResult>(result);
                Assert.Same(resultObject, okResult.Value);
            }
        }

        public class UpdateAsync : AddressControllerTest
        {

            [Fact]
            public async void Should_return_BadRequest_when_Id_is_zero()
            {
                //Arrange
                var unexistingUserId = 0;
                List<Address> address = new List<Address>() { AddressData };

                var key = "Address_StatusCode_400_BadRequest";
                var localizedString = new LocalizedString(key, "Bad Request");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                //Act
                var result = await ControllerUnderTest.Update(unexistingUserId, address);

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);

            }

            [Fact]
            public async void Should_return_NotFoundRequest_when_id_is_invalid()
            {
                //Arrange
                var unexistingUserId = 2;
                List<Address> address = new List<Address>() { AddressData };

                var key = "Address_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                //Act
                var result = await ControllerUnderTest.Update(unexistingUserId, address);

                //Assert
                Assert.IsType<NotFoundObjectResult>(result);

            }
            [Fact]
            public async void Should_return_BadResult_when_model_is_null()
            {
                List<Address> address = null;
                var userId = 6;
                var returnExistValue = true;
                var key = "Address_StatusCode_400_BadRequest";
                var localizedString = new LocalizedString(key, "Bad Request");


                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                // Service
                UserServiceMock.Setup(x => x.IsExistAsync(userId)).ReturnsAsync(returnExistValue);

                //Act 
                var result = await ControllerUnderTest.Update(userId, address);

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);


            }
            [Fact]
            public async void Should_return_OkResult_when_model_is_valid()
            {
                //Arrange
                var userId = 6;
                List<Address> address = new List<Address>() { AddressData };
                var returnExistValue = true;

                // Service
                UserServiceMock.Setup(x => x.IsExistAsync(userId)).ReturnsAsync(returnExistValue);

                //Act 
                var result = await ControllerUnderTest.Update(userId, address);

                //Assert
                Assert.IsType<OkResult>(result);
            }

        }

        public class ReadAsync : AddressControllerTest
        {
            [Fact]
            public async void Should_return_OkObjectResult_with_a_id_valid()
            {
                // Arrange
                var userId = 1;
                List<Address> addresses = new List<Address>() { AddressData };
                AddressServiceMock
                    .Setup(x => x.ReadAsync(userId))
                    .ReturnsAsync(addresses);

                // Act
                var result = await ControllerUnderTest.Get(userId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(addresses, okResult.Value);
            }
        }

        public class ReadUserAddress : AddressControllerTest
        {
            //GetUserAddress
            [Fact]
            public async void Should_return_model_when_a_addressId_valid()
            {
                // Arrange 
                var userId = 1;
                var existingAddressId = 2;
                Address address = AddressData;
                AddressServiceMock
                 .Setup(x => x.ReadOneAsync(userId, existingAddressId)).ReturnsAsync(address);

                // Act

                var result = await ControllerUnderTest.GetUserAddress(userId, existingAddressId);
                // Assert 

                Assert.IsType<OkObjectResult>(result);
            }
            [Fact]
            public async void Should_return_NoContent_when_a_addressId_is_Invalid()
            {
                // Arrange 
                var userId = 1;
                var unExistingAddressId = 55;
                Address address = null;
                AddressServiceMock
                 .Setup(x => x.ReadOneAsync(userId, unExistingAddressId)).ReturnsAsync(address);


                var key = "Address_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                LocalizerMock
                    .Setup(_ => _[key])
                         .Returns(localizedString);
                // Act

                var result = await ControllerUnderTest.GetUserAddress(userId, unExistingAddressId);
                // Assert 

                Assert.IsType<NotFoundObjectResult>(result);

            }


        }

        public class UpdateSingleAddress : AddressControllerTest
        {
            //UpdateUserAddress
            [Fact]
            public async void Should_return_BadRequest_when_addressId_is_zero()
            {
                //Arrange
                var userId = 0;
                var unExistingAddressId = 0;
                var address = AddressData;

                var key = "Address_StatusCode_400_BadRequest";
                var localizedString = new LocalizedString(key, "Bad Request");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                //Act
                var result = await ControllerUnderTest.UpdateUserAddress(userId, unExistingAddressId, address);

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);


            }
            [Fact]
            public async void Should_return_NotFoundRequest_when_addressId_is_invalid()
            {
                //Arrange
                var userId = 2;
                var addressId = 18;
                List<Address> addresses = Addresses;
                var addressData = AddressData;

                var key = "Address_StatusCode_404_NotFound";
                var localizedString = new LocalizedString(key, "Not Found");

                LocalizerMock
                  .Setup(_ => _[key])
                  .Returns(localizedString);

                //Act
                var result = await ControllerUnderTest.UpdateUserAddress(userId, addressId, addressData);

                //Assert
                Assert.IsType<NotFoundObjectResult>(result);

            }
            [Fact]
            public async void Should_return_OkResult_when_addressModel_is_valid()
            {

                //Arrange
                var userId = 1;
                var addressId = 23;
                var returnExistValue = true;

                // Service
                UserServiceMock.Setup(x => x.IsExistAsync(userId)).ReturnsAsync(returnExistValue);

                //Act 
                var result = await ControllerUnderTest.UpdateUserAddress(userId, addressId, AddressData);

                //Assert
                Assert.IsType<OkResult>(result);
            }
        }

        protected static Address AddressData => new Address()
        {
            Address1 = "1601 Hood Avenue",
            Address2 = "dolore in adipisicing et",
            City = "San Diego",
            CountryCode = "11",
            PinCode = "92103",
            StateId = 1
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
