using Ecom.DataAccess.Repository.IRepository;
using Ecom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderItem entity)
        {
             _db.OrderItems.Update(entity);
        }
    }
}