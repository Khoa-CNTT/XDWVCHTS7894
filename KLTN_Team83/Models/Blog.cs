using System.ComponentModel.DataAnnotations;

namespace KLTN_Team83.Models
{
    public class Blog
    {
        [Key]
        public int id_Blog { get; set; }
        [Required]
        public int id_Expert { get; set; }
        public string tilte { get; set; }
        public string content { get; set; }
        public string img { get; set; }
        public DateTime ngayTao { get; set; }
    }
}
