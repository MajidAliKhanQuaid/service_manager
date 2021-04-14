using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceManager.Common.Models
{
    public class CommandResponse
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        [MaxLength(100)]
        public string Command { get; set; }
        public Guid ServiceRequestCommandId { get; set; }
        public CommandRequest ServiceRequestCommand { get; set; }
        public string MachineIdentifier { get; set; }
        public string ConsoleMessage { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
