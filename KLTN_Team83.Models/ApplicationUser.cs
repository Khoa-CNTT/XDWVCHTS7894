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

        public string? Height { get; set; }
        public string? Weight { get; set; }
        public string? Age { get; set; }
        public string? Gender { get; set; }
    }
}
