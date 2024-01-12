using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public class OrderAddressRepository : Repository<OrderAddress>, IOrderAddressRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderAddressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderAddress entity)
        {
             _db.OrderAddress.Update(entity);
        }
    }
}