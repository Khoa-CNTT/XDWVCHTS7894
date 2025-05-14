using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models
{
    public class ProductImage
    {
        [Key]
        public int Id_ProductImg { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int Id_Product { get; set; }
        [ForeignKey("Id_Product")]
        public virtual Product Product { get; set; }
        
    }
}
