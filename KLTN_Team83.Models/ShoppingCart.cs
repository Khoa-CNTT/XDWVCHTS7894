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
    public class ShoppingCart
    {
        [Key]
        public int Id_SP { get; set; }
        [Required]
        public int Id_Product { get; set; }
        [ForeignKey("Id_Product")]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1, 1000,ErrorMessage ="Please enter a value between 1 and 1000")]
        public int Count { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
