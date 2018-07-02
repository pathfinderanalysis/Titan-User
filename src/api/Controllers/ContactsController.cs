using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Resources;
using Titan.UFC.Users.WebAPI.Services;

namespace Titan.UFC.Users.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/users/{userId}/contacts")]
   
    public class ContactsController : BaseController
    {
        private readonly IContactsService _contactService;
        public ContactsController(IContactsService contactService, ILogger<ContactsController> logger, ISharedResource localizer)
            : base(logger, localizer)
        {
            _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        }


        // GET: api/Users/5/Contacts/phonenumber
        [Route("phonenumber")]
        [HttpGet]
        [ProducesResponseType(typeof(Phone), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserPhone(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {ID}", userId);
            try
            {
                var phone = await _contactService.ReadOnePhoneAsync(userId);
                return Ok(phone);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.GetItemNotFound, ex, "GetUser({ID}) NOT FOUND", userId);
                return ResponseResult(StatusCodes.Status404NotFound, "Contact_StatusCode_404_NotFound");
            }
        }

        [Route("phonenumber")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int userId)
        {
            _logger.LogInformation(LoggerEvents.InsertItem, "Delete({0})", userId.ToString());

            try
            {
                await _contactService.RemovePhoneAsync(userId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Delete({0})", userId.ToString());
                return ResponseResult(StatusCodes.Status404NotFound, "Contact_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Delete({0})", userId.ToString());
                return ResponseResult(StatusCodes.Status500InternalServerError, "Contact_StatusCode_500_InternalServerError");
            }
        }


        [Route("email")]
        [HttpGet]
        [ProducesResponseType(typeof(Phone), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserEmail(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {ID}", userId);
            try
            {
                var email = await _contactService.ReadOneEmailAsync(userId);
                return Ok(email);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.GetItemNotFound, ex, "GetUser({ID}) NOT FOUND", userId);
                return ResponseResult(StatusCodes.Status404NotFound, "Contact_StatusCode_404_NotFound");
            }
        }

        [Route("email")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserEmail(int userId)
        {
            _logger.LogInformation(LoggerEvents.DeleteItem, "Delete({0})", userId.ToString());

            try
            {
                await _contactService.RemoveEmailAsync(userId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(LoggerEvents.InsertItem, ex, "Delete({0})", userId.ToString());
                return ResponseResult(StatusCodes.Status404NotFound, "Contact_StatusCode_404_NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggerEvents.DeleteItem, ex, "Delete({0})", userId.ToString());
                return ResponseResult(StatusCodes.Status500InternalServerError, "Contact_StatusCode_500_InternalServerError");
            }
        }
    }
}