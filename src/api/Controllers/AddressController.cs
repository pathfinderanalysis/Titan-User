using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Resources;
using Titan.UFC.Users.WebAPI.Services;
using Titan.UFC.Users.WebAPI.Validation;

namespace Titan.UFC.Users.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/users/{userId}/address")]
    public class AddressController : BaseController
    {
        private readonly IAddressService _addressService;
        private readonly IUserService _userService;

        public AddressController(IAddressService addressService, IUserService userService, ILogger<AddressController> logger, ISharedResource localizer)
            : base(logger, localizer)
        {
            _addressService = addressService ?? throw new ArgumentNullException(nameof(addressService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        /// <summary>
        /// Add address for existing user
        /// </summary>
        /// <param name="userId">userId Parameter.</param>
        /// <param name="address">addres object</param>
        /// <returns>added Address object</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Address))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> CreateAddress(int userId, [FromBody] Address address)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "CreateAddress({0})", JsonConvert.SerializeObject(address));
            if (address == null)
                return ResponseResult(StatusCodes.Status400BadRequest, "Address_StatusCode_400_BadRequest");

            // check user is exist for adding address else return user not found 
            var isExist = await _userService.IsExistAsync(userId);

            if (!isExist)
                return ResponseResult(StatusCodes.Status404NotFound, "Address_StatusCode_404_NotFound");

            try
            {
                var result = await _addressService.CreateAsync(userId, address);
                return Created(string.Empty, result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "CreateAddress({0})", JsonConvert.SerializeObject(address));
                return ResponseResult(StatusCodes.Status500InternalServerError, "Address_StatusCode_500_InternalServerError");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {ID}", userId);
            try
            {
                var address = await _addressService.ReadAsync(userId);
                return Ok(address);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.GetItemNotFound, ex, "GetAddress({ID}) NOT FOUND", userId);
                return ResponseResult(StatusCodes.Status404NotFound, "Address_StatusCode_404_NotFound");
            }
        }
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int userId, [FromBody] List<Address> addresses)
        {

            _logger.LogInformation(LoggerEvents.UpdateItem, "Update({userId},{addresses})", userId.ToString(), JsonConvert.SerializeObject(addresses));

            if (addresses == null || userId == 0)
                return ResponseResult(StatusCodes.Status400BadRequest, "Address_StatusCode_400_BadRequest");

            try
            {
                var isExist = await _userService.IsExistAsync(userId);

                if (!isExist)
                    return ResponseResult(StatusCodes.Status404NotFound, "Address_StatusCode_404_NotFound");

                await _addressService.UpdateAsync(userId, addresses);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.UpdateItem, ex, "Update({userId},{addresses})", userId.ToString(), JsonConvert.SerializeObject(addresses));
                return ResponseResult(StatusCodes.Status500InternalServerError, "Address_StatusCode_500_InternalServerError");
            }

        }
        [Route("{addressId}")]
        [HttpGet]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserAddress(int userId, int addressId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Get({userId},{addressId})", userId.ToString(), addressId.ToString());
            try
            {
                var address = await _addressService.ReadOneAsync(userId, addressId);
                if (address!=null)
                    return Ok(address);

                return ResponseResult(StatusCodes.Status404NotFound, "Address_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.GetItem, ex, "GetUserAddress({userId},{addressId})", userId.ToString(), addressId.ToString());
                return ResponseResult(StatusCodes.Status500InternalServerError, "Address_StatusCode_500_InternalServerError");
            }
        }
        [Route("{addressId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult>UpdateUserAddress(int userId,int addressId, [FromBody] Address address)
        {

            _logger.LogInformation(LoggerEvents.UpdateItem, "UpdateUserAddress({userId},{addressId},{addresses})", userId.ToString(),addressId.ToString(), JsonConvert.SerializeObject(address));

            if (address == null || userId == 0|| addressId==0)
                return ResponseResult(StatusCodes.Status400BadRequest, "Address_StatusCode_400_BadRequest");

            try
            {
                var isExist = await _userService.IsExistAsync(userId);

                if (!isExist)
                    return ResponseResult(StatusCodes.Status404NotFound, "Address_StatusCode_404_NotFound");

                await _addressService.UpdateAsync(userId,addressId, address);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.UpdateItem, ex, "UpdateUserAddress({userId},{addressId},{addresses})", userId.ToString(),addressId.ToString(), JsonConvert.SerializeObject(address));
                return ResponseResult(StatusCodes.Status500InternalServerError, "Address_StatusCode_500_InternalServerError");
            }

        }

    }
}