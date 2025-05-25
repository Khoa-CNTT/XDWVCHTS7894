using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace KLTN_Team83.Models
{
    //public enum GoalType { Hydration, Sleep, Weight, Steps }
    public class Goal
    {
        [Key]
        public int Id_Goal { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public int Id_GoalType { get; set; }
        [ForeignKey("Id_GoalType")]
        [ValidateNever]
        public GoalType GoalType { get; set; }
        public double TargetValue { get; set; }
        public DateTime TargetDate { get; set; }
    }
}
