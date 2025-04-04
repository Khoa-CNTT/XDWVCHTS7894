using System.ComponentModel.DataAnnotations;

namespace KLTN_Team83.Models
{
    public class Account
    {
        [Key]
        public int id_Acc { get; set; }

        [Required]
        public string loginName { get; set; }
        public string passWord { get; set; }
        public string role { get; set; }

    }
}
