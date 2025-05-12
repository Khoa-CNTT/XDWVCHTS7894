using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace KLTN_Team83.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ConversationId { get; set; }
        [ForeignKey("ConversationId")]
        public Conversation Conversation { get; set; } // Navigation property
        public string SenderId { get; set; } // UserId của người gửi
        [ForeignKey("SenderId")]
        public ApplicationUser Sender { get; set; }
        public string SenderName { get; set; } // Tên hiển thị người gửi
        public string ReceiverId { get; set; } // UserId của người nhận (tùy chọn)
        [ForeignKey("ReceiverId")]
        public ApplicationUser Receiver { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; } // Tùy chọn
    }
}
