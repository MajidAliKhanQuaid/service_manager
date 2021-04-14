using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Web.ViewModels
{
    public class SystemServiceViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Project")]
        public string ProjectId { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Machine")]
        public string MachineId { get; set; }
    }
}
