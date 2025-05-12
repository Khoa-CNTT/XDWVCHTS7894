using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models.ViewModels
{
    public class DashboardVM
    {
        public ApplicationUser User { get; set; }
        public List<Goal> Goals { get; set; }
        public List<Habit> Habits { get; set; }
        public List<HabitEntry> TodayHabitEntries { get; set; }
        public List<ScheduleItem> TodaySchedule { get; set; }
    }
}
