using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceManager.Common.Models
{
    public class SystemRole : IdentityRole
    {
        [StringLength(500)]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
