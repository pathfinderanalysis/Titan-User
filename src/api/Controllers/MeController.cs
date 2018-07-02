using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Resources;
using Titan.UFC.Users.WebAPI.Services;
using Titan.UFC.Users.WebAPI.Validation;

namespace Titan.UFC.Users.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("me")]
    public class MeController : BaseController
    {
        private readonly IUserService _userService;

        public MeController(IUserService userService, ILogger<UserController> logger, ISharedResource localizer)
            : base(logger, localizer)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item");

            int id = 0;

            try
            {
                id = this.getUserIdFromHeader();
                _logger.LogInformation(LoggerEvents.GetItem, "Getting item {ID}", id);

                var user = await _userService.ReadOneAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.GetItemNotFound, ex, "GetUser() {ID} NOT FOUND", id);
                return ResponseResult(StatusCodes.Status404NotFound, "Me_StatusCode_404_NotFound");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody] UserUpdate user)
        {
            int userId = 0;
            try
            {
                userId = getUserIdFromHeader();

                if (user == null || userId == 0)
                    return ResponseResult(StatusCodes.Status400BadRequest, "Me_StatusCode_400_BadRequest");

                _logger.LogInformation(LoggerEvents.InsertItem, "Update({0},{1})", userId.ToString(), JsonConvert.SerializeObject(user));

                var isExist = await _userService.IsExistAsync(userId);

                if (!isExist)
                    return ResponseResult(StatusCodes.Status404NotFound, "Me_StatusCode_404_NotFound");

                user.UserID = userId;
                await _userService.UpdateAsync(user);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Delete({0})", userId.ToString());
                return ResponseResult(StatusCodes.Status404NotFound, "Me_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.UpdateItem, ex, "Update({0},{1})", userId.ToString(), JsonConvert.SerializeObject(user));
                return ResponseResult(StatusCodes.Status500InternalServerError, "Me_StatusCode_500_InternalServerError");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete()
        {
            int userId = 0;
            try
            {
                userId = getUserIdFromHeader();
                _logger.LogInformation(LoggerEvents.InsertItem, "Delete({0})", userId.ToString());
                await _userService.RemoveAsync(userId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Delete({0})", userId.ToString());
                return ResponseResult(StatusCodes.Status404NotFound, "Me_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Delete({0})", userId.ToString());
                return ResponseResult(StatusCodes.Status500InternalServerError, "Me_StatusCode_500_InternalServerError");
            }
        }
        [HttpGet]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser()
        {
            int userId = 0;
            try
            {
                userId = getUserIdFromHeader();
                _logger.LogInformation(LoggerEvents.GetItem, "Getting item {ID}", userId);
                var user = await _userService.ReadOneAsync(userId);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Delete({0})", userId.ToString());
                return ResponseResult(StatusCodes.Status404NotFound, "Me_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.GetItemNotFound, ex, "GetUser({ID}) NOT FOUND", userId);
                return ResponseResult(StatusCodes.Status404NotFound, "Me_StatusCode_404_NotFound");
            }
        }

        private int getUserIdFromHeader()
        {
            if (!int.TryParse(this.Request.Headers["id"], out int id))
                throw new ArgumentNullException("id");

            return id;
        }
    }
}