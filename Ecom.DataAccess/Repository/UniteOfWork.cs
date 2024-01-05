using Ecom.DataAccess.Repository.IRepository;

namespace Ecom.DataAccess.Repository
{
    public class UniteOfWork : IUniteOfWork
    {
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }
        public IProductImageRepository ProductImage { get; }
        public IApplicationUserRepository ApplicationUser { get; }

        public IShoppingCartRepository ShoppingCart { get; }

        private readonly ApplicationDbContext _db;

        public UniteOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ProductImage = new ProductImageRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
