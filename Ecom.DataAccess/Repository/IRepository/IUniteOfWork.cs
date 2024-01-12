using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository.IRepository
{
    public interface IUniteOfWork
    {
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }
        public IProductImageRepository ProductImage { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public IShoppingCartRepository ShoppingCart { get; }
        public IOrderAddressRepository OrderAddress { get; }
        public IOrderItemRepository OrderItem { get; }
        void Save();
    }
}
