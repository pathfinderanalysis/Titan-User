using System.ComponentModel.DataAnnotations;

namespace Titan.UFC.Users.WebAPI.Models
{
    public class Email
    {
        [Required(ErrorMessage = "User_Email_EmailAddress_Required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",ErrorMessage ="User_Email_EmailAddress_ValidEmail")]
        public string Address { get; set; }
        [Required(ErrorMessage = "User_Email_UserEmailID_Required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "User_Email_IsVerified_Required")]
        public bool IsVerified { get; set; }
    }
}
