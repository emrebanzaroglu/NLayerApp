using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsWithCategory()
        {
            //Include emtodyla birlikte Eager Loading yapıldı
            //Eğer baştan değil de ihtiyaç olduğunda çekseydik lazy loading olurdu
            return await _context.Products.Include(x => x.Category).ToListAsync();  //product ile bağlı olduğu kategoriyi de çekiyoruz
        }
    }
}
