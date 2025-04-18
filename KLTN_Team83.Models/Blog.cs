using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KLTN_Team83.Models
{
    public class Blog
    {
        [Key]
        public int id_Blog { get; set; }
        [Required]

        [DisplayName("Tilte")]
        [MaxLength(200, ErrorMessage = "Tiêu đề không được quá 200 ký tự")]
        public string tilte { get; set; }

        [DisplayName("Content")]
        [MaxLength(3000, ErrorMessage = "Nội dung không được quá 3000 ký tự")]
        public string content { get; set; }

        [DisplayName("Image")]   

        public string img { get; set; }
        public DateTime ngayTao { get; set; }
    }
}
