using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerManagementSystem.Models.Entities
{
    public class Customer : BaseEntity
    {
        [Required]
        [MaxLength(128, ErrorMessage = "This field is does not support more than 128 characters")]
        [RegularExpression("[a-zA-Z ]*$")]
        [Column(TypeName = "nvarchar(128)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(128)")]
        public string Address { get; set; }
        
        [Column(TypeName = "nvarchar(64)")]
        public string PhoneNumber { get; set; }
        
        [Column(TypeName = "nvarchar(128)")]
        [Display(Name ="Profile Picture")] 
        public string ProfilePicture { get; set; }
        
        [Column(TypeName = "nvarchar(64)")]
        public string EmailAddress { get; set; }
    }
}