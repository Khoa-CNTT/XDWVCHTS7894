using System.ComponentModel.DataAnnotations;

namespace KLTN_Team83.Models
{
    public class Expert
    {
        [Key]
        public int id_Expert { get; set; }
        [Required]
        public int id_Acc { get; set; }
        [Required]
        public string expertName { get; set; }
        public string email { get; set; }
        public string capBac { get; set; }
    }
}
