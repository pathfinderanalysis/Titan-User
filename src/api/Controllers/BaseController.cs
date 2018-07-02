using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Titan.UFC.Users.WebAPI.Resources;

namespace Titan.UFC.Users.WebAPI.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ISharedResource _localizer;
        protected readonly ILogger _logger;

        public BaseController(ILogger logger, ISharedResource localizer)
        {
            _localizer = localizer;
            _logger = logger;
        }

        #region Private Helper
        protected ObjectResult ResponseResult(int statusCode,string message ,object result = null)
        {
            var objectResult = new ObjectResult(new { status = statusCode, message = result });

            switch (statusCode)
            {
                case StatusCodes.Status404NotFound:
                    objectResult = NotFound(new { status = statusCode, message = _localizer[message].Value });
                    break;
                case StatusCodes.Status400BadRequest:
                    objectResult = BadRequest(new { status = statusCode, message = _localizer[message].Value });
                    break;
                case StatusCodes.Status500InternalServerError:
                    objectResult = StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = _localizer["User_StatusCode_500_InternalServerError"].Value });
                    break;
                case StatusCodes.Status201Created:
                    objectResult = new ObjectResult(new { status = statusCode, message = result });
                    break;
                default:
                    break;
            }
            return objectResult;
        }
        #endregion

    }
}