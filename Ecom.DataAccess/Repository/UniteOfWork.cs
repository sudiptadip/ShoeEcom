using Ecom.DataAccess.Repository.IRepository;

namespace Ecom.DataAccess.Repository
{
    public class UniteOfWork : IUniteOfWork
    {
        public ICategoryRepository Category { get; }

        public IProductRepository Product { get; }

        private readonly ApplicationDbContext _db;

        public UniteOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
