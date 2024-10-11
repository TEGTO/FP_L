using FP_L.Domain.Product;
using FP_L.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FP_L.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop" },
            new Product { Id = 2, Name = "Smartphone" },
            new Product { Id = 3, Name = "Tablet" },
            new Product { Id = 4, Name = "Headphones" },
            new Product { Id = 5, Name = "Smartwatch" }
        };

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<ProductResponse> Get(int id)
        {
            var product = products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var response = new ProductResponse()
            {
                Id = id,
                Name = id + " " + product.Name
            };
            return Ok(response);
        }
        [HttpGet("db/{id}")]
        public ActionResult<ProductResponse> GetFromDb(int id)
        {
            var product = productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            var response = new ProductResponse()
            {
                Id = id,
                Name = id + " " + product.Name
            };
            return Ok(response);
        }
    }
}