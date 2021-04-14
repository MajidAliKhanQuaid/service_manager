using ServiceManager.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Web.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            Services = new List<SystemService>();
        }
        public string Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public IEnumerable<SystemService> Services { get; set; }
    }
}
