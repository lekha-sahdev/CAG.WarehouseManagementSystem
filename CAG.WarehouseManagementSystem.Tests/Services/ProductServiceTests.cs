using AutoMapper;
using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.ExceptionManagement;
using CAG.WarehouseManagementSystem.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CAG.WarehouseManagementSystem.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repoMock = new Mock<IRepository<Product>>();
            _mapperMock = new Mock<IMapper>();
            _service = new ProductService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsMappedDtos()
        {
            // Arrange
            var products = new List<Product> { 
                new Product { Id = 1, ProductCode = "PC1", Dimensions = "23X45", Title = "Product1" }, 
                new Product { Id = 2, ProductCode = "PC2", Dimensions = "24X45", Title = "Product2" } };
            var productDtos = new List<ProductDto> { 
                new ProductDto {ProductCode = "A", Title = "T", Dimensions = "D" }, 
                new ProductDto {ProductCode = "B", Title = "T2", Dimensions = "D2" } };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.Equal(2, ((List<ProductDto>)result).Count);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsMappedDto_WhenFound()
        {
            // Arrange
            var product = new Product { Id = 1, ProductCode = "PC1", Dimensions = "23X45", Title = "Product1" };
            var productDto = new ProductDto {ProductCode = "PC1", Title = "T", Dimensions = "D" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _service.GetProductByIdAsync(1);

            // Assert
            Assert.Equal("PC1", result.ProductCode);
        }

        [Fact]
        public async Task GetProductByIdAsync_Throws_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<CagBusinessException>(() => _service.GetProductByIdAsync(99));
        }

        [Fact]
        public async Task CreateProductAsync_ReturnsMappedDto()
        {
            // Arrange
            var productDto = new ProductDto {ProductCode = "PC1", Title = "T", Dimensions = "D" };
            var product = new Product { Id = 1, ProductCode = "PC1", Dimensions = "23X45", Title = "Product1" };
            _mapperMock.Setup(m => m.Map<Product>(productDto)).Returns(product);
            _repoMock.Setup(r => r.AddAsync(product)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _service.CreateProductAsync(productDto);

            // Assert
            Assert.Equal("PC1", result.ProductCode);
        }

        [Fact]
        public async Task UpdateProductAsync_ReturnsTrue_WhenUpdateSucceeds()
        {
            // Arrange
            var updateDto = new UpdateProductDto { Title = "T", Dimensions = "D" };
            var product = new Product { Id = 1, ProductCode = "PC1", Dimensions = "23X45", Title = "Product1" };
            _mapperMock.Setup(m => m.Map<Product>(updateDto)).Returns(product);
            _repoMock.Setup(r => r.UpdateAsync(It.Is<Product>(p => p.Id == 1))).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateProductAsync(1, updateDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateProductAsync_ReturnsFalse_WhenUpdateFails()
        {
            // Arrange
            var updateDto = new UpdateProductDto { Title = "T", Dimensions = "D" };
            var product = new Product { Id = 1, ProductCode = "PC1", Dimensions = "23X45", Title = "Product1" };
            _mapperMock.Setup(m => m.Map<Product>(updateDto)).Returns(product);
            _repoMock.Setup(r => r.UpdateAsync(It.Is<Product>(p => p.Id == 1))).ReturnsAsync(false);

            // Act
            var result = await _service.UpdateProductAsync(1, updateDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteProductAsync_ReturnsTrue_WhenDeleteSucceeds()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteProductAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductAsync_ReturnsFalse_WhenDeleteFails()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteProductAsync(1);

            // Assert
            Assert.False(result);
        }
    }
}
