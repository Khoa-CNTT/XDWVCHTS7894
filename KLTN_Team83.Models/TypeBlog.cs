using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models
{
    public class TypeBlog
    {
        [Key]
        public int id_TypeBlog { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
