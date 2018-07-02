using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Titan.UFC.Users.WebAPI.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "User_User_AuthRefID_Required")]
        public Guid AuthRefID { get; set; }

        [Required(ErrorMessage = "User_User_FirstName_Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "User_User_MiddleName_Required")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "User_User_LastName_Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User_User_IsEnabled_Required")]
        public bool IsEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [Required(ErrorMessage = "User_User_Locale_Required")]
        public string Locale { get; set; }
        public string DateFormat { get; set; }

        [Required(ErrorMessage = "User_User_TimeFormat_Required")]
        public string TimeFormat { get; set; }
        [Required(ErrorMessage = "User_User_TimeZone_Required")]
        public string TimeZone { get; set; }

        [Required(ErrorMessage = "User_User_PhotoUri_Required")]
        public string PhotoUri { get; set; }
        public DateTime? LastLoginTimeStamp { get; set; }
        public List<Address> Addresses { get; set; }
        [Required]
        public Contacts Contacts { get; set; }
        public string Password { get; set; }
    }
}
