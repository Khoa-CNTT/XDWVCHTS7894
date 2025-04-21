using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KLTN_Team83.Models
{
    public class InfoUser
    {
        [Key]
        public int id_User { get; set; }

        [Required]
        public int id_Acc { get; set; }
        [ForeignKey("id_Acc")]
        public Account Account { get; set; }

        [Required]
        [Display(Name = "Họ và tên")]
        public string fullName { get; set; }
        [Required]
        [Display(Name = "Tuổi")]
        public int age { get; set; }
        [Required]
        [Display(Name = "Giới tính")]
        public string gender { get; set; }
        [Required]
        [Display(Name = "Chiều cao")]
        [Range(1, 300, ErrorMessage = "Chiều cao không hợp lệ")]
        public double height { get; set; }
        [Required]
        [Display(Name = "Cân nặng")]
        [Range(1, 200, ErrorMessage = "Cân nặng không hợp lệ")]
        public double weight { get; set; }
    }
}
