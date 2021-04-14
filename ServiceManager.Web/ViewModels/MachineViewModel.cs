using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Web.ViewModels
{
    public class MachineViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(1000)]
        [MaxLength(1000)]
        public string Specs { get; set; }
        [Required]
        [StringLength(100)]
        public string Identifier { get; set; }
        //[Display(Name = "Services")]
        //public List<string> ServiceIds { get; set; }
        public List<SelectListItem> Services { get; set; }
    }
}
