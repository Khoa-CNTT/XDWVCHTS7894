using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models
{
    public class Company
    {
        [Key]
        public int Id_Company { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Logo { get; set; }
    }
}
