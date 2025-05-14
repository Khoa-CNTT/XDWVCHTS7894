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
    public class Product
    {
        [Key]
        public int Id_Product { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string NameProduct { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "NhaCungCap")]
        public string NCC { get; set; }
        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1,1000)]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }
        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        public int Id_Category { get; set; }
        [ForeignKey("Id_Category")]
        [ValidateNever]
        public Category Category { get; set; }
        
        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }   
    }
}
