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
        [Required(ErrorMessage = "Vui lòng nhập tên mục tiêu.")]
        [StringLength(200)]
        public string Title { get; set; } // Ví dụ: "Giảm 5kg", "Chạy bộ 3 lần/tuần", "Uống đủ 2 lít nước mỗi ngày"
        [StringLength(500)]
        public string? Description { get; set; } // Mô tả chi tiết hơn về mục tiêu
        public DateTime StartDate { get; set; } = DateTime.UtcNow; // Ngày bắt đầu mục tiêu

        public DateTime TargetDate { get; set; } // Ngày dự kiến hoàn thành (có thể null)
        // Để theo dõi mục tiêu có thể đo lường được (ví dụ: giảm cân, số lần thực hiện)
        public double? TargetValue { get; set; } // Ví dụ: 5 (kg), 3 (lần)
        public double? CurrentValue { get; set; }
        public string? Unit { get; set; } // Ví dụ: "kg", "lần", "phút"
        public GoalStatus Status { get; set; } = GoalStatus.InProgress; // Trạng thái mục tiêu

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Một mục tiêu có thể có nhiều thói quen liên quan
        public virtual ICollection<Habit> Habits { get; set; } = new List<Habit>();
    }
    public enum GoalStatus
    {
        InProgress, // Đang tiến hành
        Achieved,   // Đã đạt được
        Paused,     // Tạm dừng
        Abandoned   // Đã từ bỏ
    }
}
