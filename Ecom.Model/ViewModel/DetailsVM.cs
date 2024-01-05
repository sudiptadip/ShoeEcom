using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Model.ViewModel
{
    public class DetailsVM
    {
        public ShoppingCart ShoppingCart { get; set; }
        public IEnumerable<Product> OtherOption { get; set; }
    }
}