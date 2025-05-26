using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models.ViewModels
{
    public class PlanVM
    {
        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
        public string? Gender { get; set; }

        [Display(Name = "Cân nặng (kg)")]
        [Required(ErrorMessage = "Vui lòng nhập cân nặng.")]
        [Range(1, 500, ErrorMessage = "Cân nặng không hợp lệ.")]
        public double? Weight { get; set; }

        [Display(Name = "Chiều cao (cm)")]
        [Required(ErrorMessage = "Vui lòng nhập chiều cao.")]
        [Range(1, 300, ErrorMessage = "Chiều cao không hợp lệ.")]
        public double? Height { get; set; }

        public double? BMI { get; set; }
        public string? BmiCategory { get; set; }

        public List<string> NutritionalSuggestions { get; set; } = new List<string>();
        public List<string> ExerciseSuggestions { get; set; } = new List<string>();
        public bool IsSubmitted { get; set; } = false;
    }
}
