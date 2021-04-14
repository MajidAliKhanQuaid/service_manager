using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceProcess;
using System.Text;

namespace ServiceManager.Common.Models
{
    public class SystemService
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        [MaxLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        [MaxLength(500)]
        public string Description { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
        [ForeignKey("ProjectId")]
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        [ForeignKey("MachineId")]
        public Guid MachineId { get; set; }
        public virtual Machine Machine { get; set; }
        [StringLength(100)]
        [MaxLength(100)]
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public DateTime LastStatusUpdatedUtc { get; set; }

    }
}
