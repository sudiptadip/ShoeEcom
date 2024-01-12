using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Model
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; } 

        public double Price { get; set; }

        public double DiscountPrice { get; set; }

        public string Size { get; set; }

        public string OrderStatus { get; set; }

        public string PaymentType { get; set; }

        public DateTime OrderTime { get; set; }

        public int ProductId { get; set; }
        [ValidateNever,ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public string ApplicationUserId { get; set; }
        [ValidateNever, ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public int OrderAddressId { get; set; }
        [ValidateNever, ForeignKey(nameof(OrderAddressId))]
        public OrderAddress OrderAddress { get; set; }
    }
}
