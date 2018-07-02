using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Resources;
using Titan.UFC.Users.WebAPI.Services;
using Titan.UFC.Users.WebAPI.Validation;

namespace Titan.UFC.Users.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IContactsService _contactService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IContactsService contactsService, ILogger<UserController> logger, ISharedResource localizer, IConfiguration configuration)
            : base(logger, localizer)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _contactService = contactsService ?? throw new ArgumentNullException(nameof(contactsService));
            _configuration = configuration;
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {ID}", id);
            try
            {
                var user = await _userService.ReadOneAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.GetItemNotFound, ex, "GetUser({ID}) NOT FOUND", id);
                return ResponseResult(StatusCodes.Status404NotFound, "User_StatusCode_404_NotFound");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "CreateUser({0})", JsonConvert.SerializeObject(user));
            if (user == null)
                return ResponseResult(StatusCodes.Status400BadRequest, "User_StatusCode_400_BadRequest");

            try
            {
                var result = await _userService.CreateAsync(user);
                return Created(string.Empty, result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "CreateUser({0})", JsonConvert.SerializeObject(user));
                return ResponseResult(StatusCodes.Status500InternalServerError, "User_StatusCode_500_InternalServerError");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ValidateModel]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdate user)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "Update({0},{1})", id.ToString(), JsonConvert.SerializeObject(user));

            if (user == null || id == 0)
                return ResponseResult(StatusCodes.Status400BadRequest, "User_StatusCode_400_BadRequest");

            try
            {
                var isExist = await _userService.IsExistAsync(id);

                if (!isExist)
                    return ResponseResult(StatusCodes.Status404NotFound, "User_StatusCode_404_NotFound");

                user.UserID = id;
                await _userService.UpdateAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.UpdateItem, ex, "Update({0},{1})", id.ToString(), JsonConvert.SerializeObject(user));
                return ResponseResult(StatusCodes.Status500InternalServerError, "User_StatusCode_500_InternalServerError");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "Delete({0})", id.ToString());

            try
            {
                await _userService.RemoveAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Delete({0})", id.ToString());
                return ResponseResult(StatusCodes.Status404NotFound, "User_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Delete({0})", id.ToString());
                return ResponseResult(StatusCodes.Status500InternalServerError, "User_StatusCode_500_InternalServerError");
            }
        }

        [Route("forgotpassword")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ForgotPassword))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateModel]
        public async Task<IActionResult> ForgotPassword([FromBody] Contacts contact)
        {
            _logger.LogInformation(LoggerEvents.IsExistItem, "Check valid user by contacts({0})", JsonConvert.SerializeObject(contact));
            try
            {
                if (contact.Email == null && contact.Phone == null)
                    return ResponseResult(StatusCodes.Status400BadRequest, "User_StatusCode_400_BadRequest");

                var validUserId = contact.Phone != null ? await _contactService.CheckValidUserByPhoneAsync(contact.Phone) :
                                  contact.Email != null ? await _contactService.CheckValidUserByEmailAsync(contact.Email) : 0;

                if (validUserId == 0)
                {
                    return Ok();
                }
                else
                {
                   await _userService.DeleteForgotPasswordOTPAsync(validUserId);

                    int.TryParse(_configuration["PasswordConfig:OTPExpiryDays"], out int expirationDays);
                    ForgotPassword forgotPassword = new ForgotPassword()
                    {
                        UserID = validUserId,
                        OTPExpiryTime = DateTime.UtcNow.AddDays(expirationDays),
                        ResetOTP = GetOTP(),
                        ResetTime = null,
                        CreatedDate = DateTime.UtcNow
                    };

                    var result = await _userService.CreateAsync(forgotPassword);

                }
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Phone or email not match({0})", JsonConvert.SerializeObject(contact));
                return ResponseResult(StatusCodes.Status404NotFound, "Contact_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "CreatePassword by contacts({0})", JsonConvert.SerializeObject(contact));
                return ResponseResult(StatusCodes.Status500InternalServerError, "User_StatusCode_500_InternalServerError");
            }
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPassword resetPassword)
        {
            _logger.LogInformation(LoggerEvents.UpdateItem, "Reset Password({resetPassword})", JsonConvert.SerializeObject(resetPassword));
            try
            {
                if (resetPassword == null)
                    return ResponseResult(StatusCodes.Status400BadRequest, "User_StatusCode_400_BadRequest");
                var result = await _userService.ResetPasswordAsync(resetPassword);
                if (result)
                    return Ok();

                return ResponseResult(StatusCodes.Status404NotFound, "User_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.UpdateItem, ex, "Reset Password ({resetPassword})", JsonConvert.SerializeObject(resetPassword));
                return ResponseResult(StatusCodes.Status500InternalServerError, "User_StatusCode_500_InternalServerError");
            }
        }

        [HttpPost]
        [Route("resetpassword/{otp}")]
        public async Task<IActionResult> ResetPassword(string otp, [FromBody] ResetPasswordViaURL newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(newPassword.NewPassword))
                    throw new ArgumentNullException("newPassword");

                _logger.LogInformation(LoggerEvents.UpdateItem, "Reset Password({0},{1})", otp, newPassword);

                if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(newPassword.NewPassword))
                {
                    return ResponseResult(StatusCodes.Status400BadRequest, "User_StatusCode_400_BadRequest");
                }

                var resetPassword = new ResetPassword()
                {
                    NewPassword = newPassword.NewPassword,
                    OTP = otp
                };
                var result = await _userService.ResetPasswordAsync(resetPassword);
                if (result)
                    return Ok();

                return ResponseResult(StatusCodes.Status404NotFound, "User_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.UpdateItem, ex, "Reset Password({0},{1})", otp, newPassword.NewPassword);
                return ResponseResult(StatusCodes.Status500InternalServerError, "User_StatusCode_500_InternalServerError");
            }
        }

        public string GetOTP()
        {
            Random generator = new Random();
            String otp = generator.Next(0, 999999).ToString("D6");
            return otp;
        }
    }
}