using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
        [MaxLength(5000, ErrorMessage = "Nội dung không được quá 5000 ký tự")]
        public string content { get; set; }

        [ValidateNever]
        public string? ImageUrl { get; set; }
        public DateTime ngayTao { get; set; }

        public int id_TypeBlog { get; set; }
        [ForeignKey("id_TypeBlog")]
        [ValidateNever]
        public TypeBlog TypeBlog { get; set; }
    }
}
