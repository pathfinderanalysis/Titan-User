using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Resources;

namespace Titan.UFC.Users.WebAPI.Validation
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState, string message) : base(new ValidationResultModel(modelState, message))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
