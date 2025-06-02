using CAG.WarehouseManagementSystem.Controllers;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAG.WarehouseManagementSystem.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ILogger<ProductController>> _loggerMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _loggerMock = new Mock<ILogger<ProductController>>();
            _controller = new ProductController(_productServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOk_WithProductList()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { ProductCode = "P1", Title = "Product 1", Dimensions = "10x10" },
                new ProductDto { ProductCode = "P2", Title = "Product 2", Dimensions = "20x20" }
            };
            _productServiceMock.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetProduct_ReturnsOk_WhenProductExists()
        {
            // Arrange
            var product = new ProductDto { ProductCode = "P1", Title = "Product 1", Dimensions = "10x10" };
            _productServiceMock.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal("P1", returnValue.ProductCode);
        }

        [Fact]
        public async Task PostProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var productDto = new ProductDto { ProductCode = "P1", Title = "Product 1", Dimensions = "10x10" };
            _productServiceMock.Setup(s => s.CreateProductAsync(productDto)).ReturnsAsync(productDto);

            // Act
            var result = await _controller.PostProduct(productDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetProduct", createdAtActionResult.ActionName);
            Assert.Equal(productDto.Id, ((ProductDto)createdAtActionResult.Value!).Id);
        }

        [Fact]
        public async Task PutProduct_ReturnsNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            var updateDto = new UpdateProductDto { Title = "Product 1", Dimensions = "10x10" };
            _productServiceMock.Setup(s => s.UpdateProductAsync(1, updateDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.PutProduct(1, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutProduct_ReturnsNotFound_WhenUpdateFails()
        {
            // Arrange
            var updateDto = new UpdateProductDto { Title = "Product 1", Dimensions = "10x10" };
            _productServiceMock.Setup(s => s.UpdateProductAsync(1, updateDto)).ReturnsAsync(false);

            // Act
            var result = await _controller.PutProduct(1, updateDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent_WhenDeleteSucceeds()
        {
            // Arrange
            _productServiceMock.Setup(s => s.DeleteProductAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenDeleteFails()
        {
            // Arrange
            _productServiceMock.Setup(s => s.DeleteProductAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
