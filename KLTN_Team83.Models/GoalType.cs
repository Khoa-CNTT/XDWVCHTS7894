using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models
{
    public class GoalType
    {
        [Key]
        public int Id_GoalType { get; set; }
        [Required]
        public string NameGoalType { get; set; }
    }
}
