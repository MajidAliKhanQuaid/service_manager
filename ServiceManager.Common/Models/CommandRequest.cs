using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceManager.Common.Models
{
    public class CommandRequest
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        [MaxLength(100)]
        public string Command { get; set; }
        public Guid ServiceId { get; set; }
        public SystemService Service { get; set; }
        public string MachineIdentifier { get; set; }
        [StringLength(100)]
        [MaxLength(100)]
        public string RequestedBy { get; set; }
        public DateTime RequestTimeUtc { get; set; }
        public DateTime? LastProcessedUtc { get; set; }
    }
}
