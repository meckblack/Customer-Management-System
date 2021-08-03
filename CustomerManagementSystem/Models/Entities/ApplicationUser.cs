using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CustomerManagementSystem.Models.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [RegularExpression("[a-zA-Z ]*$")]
        [Column(TypeName = "nvarchar(128)")]
        public string Name { get; set; }

        public string Address { get; set; }
        
        [Column(TypeName = "nvarchar(64)")]
        [Display(Name ="Profile Picture")] 
        public string ProfilePicture { get; set; }
        
        
    }
}