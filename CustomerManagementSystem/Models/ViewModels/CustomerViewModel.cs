using CustomerManagementSystem.Models.Entities;

namespace CustomerManagementSystem.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Address { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string ProfilePicture { get; set; }
        
        public string EmailAddress { get; set; }
    }
}