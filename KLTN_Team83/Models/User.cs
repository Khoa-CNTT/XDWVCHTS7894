using System.ComponentModel.DataAnnotations;

namespace KLTN_Team83.Models
{
    public class User
    {
        [Key]
        public int id_User { get; set; }

        [Required]
        public int id_Acc { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public float height { get; set; }
        public float weight { get; set; }
    }
}
