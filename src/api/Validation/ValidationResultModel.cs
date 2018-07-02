using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Titan.UFC.Users.WebAPI.Validation
{
    public class ValidationResultModel
    {
        public string Message { get; }

        public List<ValidationError> Errors { get; }

        public ValidationResultModel(ModelStateDictionary modelState, string message)
        {
            Message = message;
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }
}
