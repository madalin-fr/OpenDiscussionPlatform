using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpenDiscussionPlatform.Models
{
    public class UserDetails
    {
        [Key]
        public string UserId { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }
        [Required]
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ApplicationUser User { get; set; }
    }

}