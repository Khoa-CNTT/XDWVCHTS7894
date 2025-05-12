using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace KLTN_Team83.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string User1Id { get; set; } // ID của người dùng 1
        [ForeignKey("User1Id")]
        public ApplicationUser User1 { get; set; }
        public string User2Id { get; set; } // ID của người dùng 2 (hoặc chuyên gia)
        [ForeignKey("User1Id")]
        public ApplicationUser User2 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public string Status { get; set; } // Ví dụ: Pending, Active, Closed
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
