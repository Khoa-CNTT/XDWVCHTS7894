using System.ComponentModel.DataAnnotations;

namespace KLTN_Team83.Models
{
    public class Admin
    {
        [Key]
        public int id_Admin { get; set; }
        [Required]
        public int id_Acc { get; set; }
        public string adminName { get; set; }
        public string email { get; set; }

    }
}
