using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Titan.UFC.Users.WebAPI.Models
{
    public class Contacts : IValidatableObject
    {     
        public Email Email { get; set; }
        public Phone Phone { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Email == null && Phone == null)
                yield return new ValidationResult("Either Email or Phone must be specified");
        }
    }
}
