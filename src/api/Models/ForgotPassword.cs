using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Titan.UFC.Users.WebAPI.Models
{
    public class ForgotPassword
    {
        public int ForgotPasswordID { get; set; }
        public int UserID { get; set; }
        public string ResetOTP { get; set; }
        public DateTime OTPExpiryTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ResetTime { get; set; }
    }
}
