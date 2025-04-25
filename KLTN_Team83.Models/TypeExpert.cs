using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models
{
    public class TypeExpert
    {
        [Key]
        public int id_TypeExpert { get; set; }
        [Required]
        public string name { get; set; }
        public List<Expert> Experts { get; set; }
    }
}
