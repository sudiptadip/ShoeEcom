using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Model.ViewModel
{
    public class DetailsVM
    {
        public Product Product { get; set; }
        public IEnumerable<Product> OtherOption { get; set; }
    }
}
