using System.ComponentModel.DataAnnotations;

namespace KLTN_Team83.Models
{
    public class User
    {
        [Key]
        public int Id_user { get; set; }

        [Required, StringLength(100)]
        public string NameUser { get; set; }

        [Required, StringLength(255)]
        public string Password { get; set; }
    }
}
