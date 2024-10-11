using FP_L.Domain.Product;
using Microsoft.EntityFrameworkCore;

namespace FP_L.Data
{
    public class ProductDbContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }

        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
