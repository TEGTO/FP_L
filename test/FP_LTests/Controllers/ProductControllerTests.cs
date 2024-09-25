using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FP_L.Controllers.Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ProductController controller;
        private Mock<ILogger<ProductController>> mockLogger;

        [SetUp]
        public void SetUp()
        {
            mockLogger = new Mock<ILogger<ProductController>>();
            controller = new ProductController(mockLogger.Object);
        }

        [Test]
        public void Get_ValidProductId_ReturnsOkObjectResult()
        {
            // Arrange
            var productId = 1;
            // Act
            var result = controller.Get(productId);
            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var productResponse = okResult.Value as ProductResponse;
            Assert.IsNotNull(productResponse);
            Assert.That(productResponse.Id, Is.EqualTo(productId));
            Assert.That(productResponse.Name, Is.EqualTo($"{productId} Laptop"));
        }
        [Test]
        public void Get_InvalidProductId_ReturnsNotFoundResult()
        {
            // Arrange
            var invalidProductId = 99;
            // Act
            var result = controller.Get(invalidProductId);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}