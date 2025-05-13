using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace KLTN_Team83.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id_OH { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public double OrderTotal { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateOnly PaymentDueDate { get; set; }
        public string? PaymentIntentId { get; set; }

        [Required]
        [Display(Name="Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Địa chỉ")]
        public string StreetAddress { get; set; }
        [Required]
        [Display(Name = "Thành phố")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Quốc gia")]
        public string State { get; set; }
        [Required]
        [Display(Name = "Mã bưu chính")]
        public string PostalCode { get; set; }
        [Required]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }
    }
}
