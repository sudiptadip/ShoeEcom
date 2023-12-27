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
        void Save();
    }
}
