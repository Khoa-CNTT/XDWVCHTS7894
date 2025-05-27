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
    public class Habit
    {
        [Key]
        public int Id_Habit { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public int? GoalId { get; set; } // FK (nullable) đến Goal nếu thói quen này thuộc về một mục tiêu cụ thể
        [ForeignKey("GoalId")]
        [ValidateNever]
        public virtual Goal? Goal { get; set; }
        //tên thói quen
        [Required(ErrorMessage = "Vui lòng nhập tên thói quen.")]
        [StringLength(200)]
        public string Title { get; set; } // Ví dụ: "Uống 1 ly nước sau khi thức dậy", "Đi bộ 30 phút buổi tối", "Không ăn vặt sau 8h tối"

        [StringLength(500)]
        public string? Description { get; set; }

        public HabitFrequency FrequencyType { get; set; } = HabitFrequency.Daily; // Tần suất: Hàng ngày, hàng tuần, cụ thể
        public List<DayOfWeek>? DaysOfWeek { get; set; } // Nếu FrequencyType = Weekly, lưu các ngày trong tuần (cần converter hoặc bảng riêng)
                                                         // Hoặc có thể lưu dạng string "Mon,Wed,Fri" rồi parse
        public int TargetCompletionsPerPeriod { get; set; } = 1; // Số lần cần hoàn thành mỗi ngày/tuần (nếu FrequencyType là Daily/Weekly)

        // Thời gian nhắc nhở (tùy chọn)
        [DataType(DataType.Time)]
        public TimeSpan? ReminderTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Danh sách các lần thực hiện thói quen
        public virtual ICollection<HabitLog> HabitLogs { get; set; } = new List<HabitLog>();

    }
    public enum HabitFrequency
    {
        Daily,  // Hàng ngày
        Weekly, // Hàng tuần (vào những ngày cụ thể)
        // Monthly, // Hàng tháng (vào ngày cụ thể)
        // SpecificDays // Những ngày cụ thể được chọn (phức tạp hơn)
    }
    
}
