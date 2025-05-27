using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace KLTN_Team83.Models
{
    public class HabitLog
    {
        public int Id { get; set; }

        [Required]
        public int Id_Habit { get; set; } // FK đến Habit
        [ForeignKey("Id_Habit")]
        [ValidateNever]
        public Habit Habit { get; set; }

        [Required]
        public string UserId { get; set; } // FK đến ApplicationUser (để dễ truy vấn)
        [ForeignKey("UserId")]
        [ValidateNever]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime LogDate { get; set; } // Ngày thực hiện
        public bool IsCompleted { get; set; } = true; // Đánh dấu đã hoàn thành (có thể mở rộng với số lượng nếu thói quen là "uống 2 lít nước")
        public string? Notes { get; set; } // Ghi chú thêm
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
