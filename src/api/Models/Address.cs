using System;
using System.ComponentModel.DataAnnotations;

namespace Titan.UFC.Users.WebAPI.Models
{
    public class Address
    {
        [Required(ErrorMessage = "User_Address_AddressID_Required")]
        public int AddressID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required(ErrorMessage = "User_Address_City_Required")]
        public string City { get; set; }
        [Required(ErrorMessage = "User_Address_StateID_Required")]
        public short? StateId { get; set; }

        [Required(ErrorMessage = "User_Address_PinCode_Required")]
        [RegularExpression("^[0-9]{5}", ErrorMessage = "User_Address_PinCode_OnlyNumbers")]
        public string PinCode { get; set; }

        public string CountryCode { get; set; }

        [Required(ErrorMessage ="User_Address_IsVerified_Required")]
        public bool IsVerified { get; set; }
    }
}
