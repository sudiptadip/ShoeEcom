using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductImage entity)
        {
             _db.Update(entity);
        }
    }
}