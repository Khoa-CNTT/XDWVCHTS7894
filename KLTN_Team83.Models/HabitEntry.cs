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
    public class HabitEntry
    {
        [Key]
        public int Id_HabitEntry { get; set; }
        [Required]
        public int HabitId { get; set; }
        [ValidateNever]
        [ForeignKey("HabitId")]
        public Habit Habit { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
    }
}
