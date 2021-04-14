using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceManager.Common.Models
{
    public class Machine
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000)]
        public string Specs { get; set; }
        [Required]
        [StringLength(100)]
        public string Identifier { get; set; }
        public ICollection<SystemService> Services { get; set; }
    }
}
