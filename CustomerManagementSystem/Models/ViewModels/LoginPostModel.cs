using System.ComponentModel.DataAnnotations;

namespace CustomerManagementSystem.Models.ViewModels
{
    public class LoginPostModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}