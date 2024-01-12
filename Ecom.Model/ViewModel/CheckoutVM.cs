using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Model.ViewModel
{
    public class CheckoutVM
    {
        [ValidateNever]
        public List<ShoppingCart> Carts { get; set; }
        public OrderAddress OrderAddress { get; set; }
    }
}
