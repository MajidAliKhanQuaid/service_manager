using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServiceManager.Common.Models
{
    [Table("Users")]
    public class SystemUser : IdentityUser
    {
        [PersonalData]
        [StringLength(100)]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [PersonalData]
        [StringLength(100)]
        [MaxLength(100)]
        public string LastName { get; set; }
        [PersonalData]
        [StringLength(100)]
        [MaxLength(100)]
        public string Team { get; set; }
        [PersonalData]
        [StringLength(100)]
        [MaxLength(100)]
        public string Designation { get; set; }
        [PersonalData]
        [StringLength(100)]
        [MaxLength(100)]
        public string Status { get; set; }

        [StringLength(100)]
        [MaxLength(100)]
        public virtual ICollection<Project> Projects { get; set; }
    }
}
