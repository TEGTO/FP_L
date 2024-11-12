using FP_L.Domain.Dtos;
using FP_L.Domain.Models;
using FP_L.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FP_L.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly ICacheService cacheService;
        private readonly ILogger<ProductController> logger;

        private readonly List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop" },
            new Product { Id = 2, Name = "Smartphone" },
            new Product { Id = 3, Name = "Tablet" },
            new Product { Id = 4, Name = "Headphones" },
            new Product { Id = 5, Name = "Smartwatch" }
        };

        public ProductController(ICacheService cacheService, ILogger<ProductController> logger)
        {
            this.cacheService = cacheService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> Get(int id)
        {
            logger.LogInformation("Get {0}", id);
            var product = await SomeLongFindFunctionAsync(id);
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
        [HttpGet("cache/{id}")]
        public async Task<ActionResult<ProductResponse>> GetWithCache(int id)
        {
            logger.LogInformation("GetWithCache {0}", id);
            var cacheKey = $"GetWithCache_{id}";
            var cache = await cacheService.GetAsync(cacheKey);
            var cachedResponse = !string.IsNullOrEmpty(cache) ? JsonSerializer.Deserialize<ProductResponse>(cache) : null;

            if (cachedResponse == null)
            {
                var product = await SomeLongFindFunctionAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                var response = new ProductResponse()
                {
                    Id = id,
                    Name = id + " " + product.Name
                };

                cachedResponse = response;
                await cacheService.SetAsync(cacheKey, JsonSerializer.Serialize(cachedResponse), TimeSpan.FromSeconds(5));
            }

            return Ok(cachedResponse);
        }
        [HttpGet("static-image")]
        public async Task<IActionResult> GetStaticImage()
        {
            logger.LogInformation("GetStaticImage");

            await Task.Delay(2500);

            var filePath = Path.Combine("wwwroot", "images", "sample-image.jpg");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Image not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg");
        }

        private async Task<Product?> SomeLongFindFunctionAsync(int id)
        {
            var product = products.Find(x => x.Id == id);

            await Task.Delay(2500);

            return product;
        }
    }
}