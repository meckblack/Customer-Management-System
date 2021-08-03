using System.ComponentModel.DataAnnotations;

namespace CustomerManagementSystem.Models.ViewModels
{
    public class CommentPostModel
    {
        [Required]
        public string Comment { get; set; }
    }
}