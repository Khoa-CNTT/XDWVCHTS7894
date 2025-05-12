using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models
{
    public enum GoalType { Hydration, Sleep, Weight, Steps }
    public class Goal
    {
        [Key]
        public int Id_Goal { get; set; }
        [Required]
        public string UserId { get; set; }
        public GoalType Type { get; set; }
        public double TargetValue { get; set; }
        public DateTime TargetDate { get; set; }
    }
}
