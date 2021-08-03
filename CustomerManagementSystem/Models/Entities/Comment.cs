using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerManagementSystem.Models.Entities
{
    public class Comment : BaseEntity
    {
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        
        public Customer Customer { get; set; }

        [Required]
        public string Message { get; set; }
        
    }
}