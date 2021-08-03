using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CustomerManagementSystem.Models.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Access { get; set; }

        [DisplayName("Created")] 
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        [NotMapped] public int UsersCount { get; set; }
    }
}