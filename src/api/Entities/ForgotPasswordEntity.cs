using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Titan.UFC.Users.WebAPI.Entities
{
    [Table("ForgotPassword", Schema = "USR")]
    public class ForgotPasswordEntity
    {
        [Key]
        public int ForgotPasswordID { get; set; }
        public int UserID { get; set; }
        public string ResetOTP { get; set; }
        public DateTime OTPExpiryTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ResetTime { get; set; }
    }
}
