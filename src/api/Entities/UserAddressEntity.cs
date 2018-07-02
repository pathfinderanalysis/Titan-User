using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Titan.UFC.Users.WebAPI.Entities
{
    [Table("User_Address", Schema = "USR")]
    public class UserAddressEntity
    {
        [Key]
        public int UserID { get; set; }
        [Key]
        public int AddressID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public AddressEntity Address { get; set; }
        public UserEntity User { get; set; }
    }
}
