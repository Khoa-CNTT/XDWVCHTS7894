 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace KLTN_Team83.Models
{
    public class Infomation
    {
        [Key]
        public int Id_User { get; set; }
        [ForeignKey("Id")]
        [ValidateNever]
        public IdentityUser Identity  { get; set; }
        [Required]
        public string? Gender { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
    }
}
