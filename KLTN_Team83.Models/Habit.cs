using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models
{
    public class Habit
    {
        [Key]
        public int Id_Habit { get; set; }
        [Required]
        public string UserId { get; set; }
        //tên thói quen
        public string Name { get; set; }

    }
}
