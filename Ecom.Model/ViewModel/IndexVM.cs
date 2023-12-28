using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Model.ViewModel
{
    public class IndexVM
    {
        public IEnumerable<Product> MensShoe {  get; set; }
        public IEnumerable<Product> WomensShoe {  get; set; }
        public IEnumerable<Product> TrendingShoe {  get; set; }
        public IEnumerable<Product> Boots {  get; set; }
    }
}