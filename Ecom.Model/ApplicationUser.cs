using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
        public string? Country { get; set; }
        
        public string? State { get; set; }
        
        public string? City { get; set; }
        
        public string? PostalCode { get; set; }
        
        public string? Address { get; set; }

        public string? ProfileImgUrl { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
