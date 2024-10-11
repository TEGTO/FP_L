using FP_L.Domain.Product;

namespace FP_L.Repository
{
    public interface IProductRepository
    {
        public Product GetProductById(int id);
    }
}