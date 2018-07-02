using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Titan.UFC.Users.WebAPI.Entities
{
    [Table("UserEmail", Schema = "USR")]
    public class UserEmailEntity
    {
        [Key]
        public int UserEmailID { get; set; }
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string VerificationKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserEntity User { get; set; }
    }
}
