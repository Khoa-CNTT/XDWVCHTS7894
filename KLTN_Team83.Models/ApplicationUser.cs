using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KLTN_Team83.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public double? Height { get; set; }
        public double? Weight { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
    }
}
