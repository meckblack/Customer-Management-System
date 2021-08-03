using System.ComponentModel.DataAnnotations;

namespace CustomerManagementSystem.Models.ViewModels
{
    public class LoginViewModel
    {
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Token { get; set; }
    }
}