using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Titan.UFC.Users.WebAPI.Entities
{
    [Table("Address", Schema = "dbo")]
    public class AddressEntity
    {
        [Key]
        public int AddressID { get; set; }
      
        public string Address1 { get; set; }
       
        public string Address2 { get; set; }
       
        public string City { get; set; }
       
        public short? StateID { get; set; }
      
        public string PinCode { get; set; }
      
        public string CountryCode { get; set; }
       
        public bool IsVerified { get; set; }
      
        public DateTime CreatedDate { get; set; }
       
        public DateTime? UpdatedDate { get; set; }

        public UserAddressEntity UserAddress { get; set; }
    }
}
