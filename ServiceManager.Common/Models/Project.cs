using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceManager.Common.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [StringLength(500)]
        [MaxLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        public virtual ICollection<SystemUser> Users { get; set; }
        public virtual ICollection<SystemService> Services { get; set; }
    }
}
