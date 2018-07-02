using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Titan.UFC.Users.WebAPI.Entities
{    
    [Table("UserMaster", Schema = "USR")]
    public class UserEntity
    {
        public UserEntity()
        {
            this.UserAddresses = new HashSet<UserAddressEntity>();
        }

        [Key]        
        public int UserID { get; set; }            
        public Guid AuthRefID { get; set; }             
        public string FirstName { get; set; }       
        public string MiddleName { get; set; }     
        public string LastName { get; set; }    
        public bool IsEnabled { get; set; }     
        public DateTime CreatedDate { get; set; }     
        public DateTime? UpdatedDate { get; set; }      
        public string Locale { get; set; }     
        public string DateFormat { get; set; }     
        public string TimeFormat { get; set; }      
        public string TimeZone { get; set; }      
        public string PhotoUri { get; set; }     
        public DateTime? LastLoginTimeStamp { get; set; }
        public ICollection<UserAddressEntity> UserAddresses { get; set; }
        public UserEmailEntity UserEmail { get; set; }
        public UserPhoneEntity UserPhone { get; set; }

    }
}
