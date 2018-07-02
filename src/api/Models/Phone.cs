using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Titan.UFC.Users.WebAPI.Models
{
    public class Phone
    {
        [Required(ErrorMessage = "User_Phone_Number_Required")]
        [RegularExpression("^[0-9]{10}", ErrorMessage = "User_Phone_Number_OnlyNumbers")]
        public string Number { get; set; }
        [Required(ErrorMessage = "User_Phone_CountryCode_Required")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "User_Phone_UserPhoneID_Required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "User_Phone_IsVerified_Required")]
        public bool IsVerified { get; set; }
    }
}
