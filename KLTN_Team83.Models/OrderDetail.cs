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
    public class OrderDetail
    {
        [Key]
        public int Id_OD { get; set; }
        public int Id_OH { get; set; }
        [ForeignKey("Id_OH")]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }

        public int Id_Product { get; set; }
        [ForeignKey("Id_Product")]
        [ValidateNever]
        public Product Product { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }

    }
}
