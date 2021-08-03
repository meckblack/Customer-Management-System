using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomerManagementSystem.Models.Entities
{
    public class BaseEntity
    {
        [Key] public int Id { get; set; }

        [DisplayName("Created")] 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DisplayName("Modified")] 
        public DateTime? LastModifiedDate { get; set; }

        public int CreatedBy { get; set; }

        public int? LastModifiedBy { get; set; }
    }
}