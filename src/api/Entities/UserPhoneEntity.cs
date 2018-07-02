using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Titan.UFC.Users.WebAPI.Entities
{
    [Table("UserPhone", Schema = "USR")]
    public class UserPhoneEntity
    {
        [Key]
        public int UserPhoneID { get; set; }
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public string PhoneNumber { get; set; }
        public string CallingCode { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string VerificationKey { get; set; }
        public UserEntity User { get; set; }
    }
}
