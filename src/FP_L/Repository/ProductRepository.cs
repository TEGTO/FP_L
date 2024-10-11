using FP_L.Data;
using FP_L.Domain.Product;
using Microsoft.EntityFrameworkCore;

namespace FP_L.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<ProductDbContext> dbContextFactory;

        public ProductRepository(IDbContextFactory<ProductDbContext> contextFactory)
        {
            this.dbContextFactory = contextFactory;

            using (var context = CreateDbContextAsync())
            {
                var existingProducts = context.Products.ToList();
                context.Products.RemoveRange(existingProducts);
                context.SaveChanges();

                List<Product> products = new List<Product>
                {
                    new Product { Id = 1, Name = "From Db Laptop" },
                    new Product { Id = 2, Name = "From Db Smartphone" },
                    new Product { Id = 3, Name = "From Db Tablet" },
                    new Product { Id = 4, Name = "From Db Headphones" },
                    new Product { Id = 5, Name = "From Db Smartwatch" }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
        public Product GetProductById(int id)
        {
            using (var context = CreateDbContextAsync())
            {
                var product = context.Products.First(x => x.Id == id);
                return product;
            }
        }

        private ProductDbContext CreateDbContextAsync()
        {
            return dbContextFactory.CreateDbContext();
        }
    }
}