using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Resources;

namespace Titan.UFC.Users.WebAPI.Validation
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var localizer = (ISharedResource)context.HttpContext.RequestServices.GetService(typeof(ISharedResource));
                context.Result = new ValidationFailedResult(context.ModelState, localizer["Validation_Failed"].Value);
            }
        }
    }
}
