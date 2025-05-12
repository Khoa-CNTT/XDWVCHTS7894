using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models
{
    public class ScheduleItem
    {
        [Key]
        public int Id_Sche { get; set; }
        [Required]
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
