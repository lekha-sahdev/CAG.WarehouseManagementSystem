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
    public class CustomerServiceTests
    {
        private readonly Mock<IRepository<Customer>> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _repoMock = new Mock<IRepository<Customer>>();
            _mapperMock = new Mock<IMapper>();
            _service = new CustomerService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsMappedDtos()
        {
            // Arrange
            var customers = new List<Customer> {
                new Customer { Id = 1, Name = "A", Address = "X" },
                new Customer { Id = 2, Name = "B", Address = "Y" } };
            var customerDtos = new List<CustomerDto> { 
                new CustomerDto { Name = "A", Address = "X" }, 
                new CustomerDto {Name = "B", Address = "Y" } };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);
            _mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers)).Returns(customerDtos);

            // Act
            var result = await _service.GetAllCustomersAsync();

            // Assert
            Assert.Equal(2, ((List<CustomerDto>)result).Count);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsMappedDto_WhenFound()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "A", Address = "X" };
            var customerDto = new CustomerDto {Name = "A", Address = "X" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
            _mapperMock.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

            // Act
            var result = await _service.GetCustomerByIdAsync(1);

            // Assert
            Assert.Equal("A", result.Name);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_Throws_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Customer)null);

            // Act & Assert
            await Assert.ThrowsAsync<CagBusinessException>(() => _service.GetCustomerByIdAsync(99));
        }

        [Fact]
        public async Task CreateCustomerAsync_ReturnsMappedDto()
        {
            // Arrange
            var customerDto = new CustomerDto {Name = "A", Address = "X" };
            var customer = new Customer { Id = 1, Name = "A", Address = "X" };
            _mapperMock.Setup(m => m.Map<Customer>(customerDto)).Returns(customer);
            _repoMock.Setup(r => r.AddAsync(customer)).ReturnsAsync(customer);
            _mapperMock.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

            // Act
            var result = await _service.CreateCustomerAsync(customerDto);

            // Assert
            Assert.Equal("A", result.Name);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsTrue_WhenUpdateSucceeds()
        {
            // Arrange
            var updateDto = new UpdateCustomerDto { Name = "A", Address = "X" };
            var customer = new Customer { Id = 1, Name = "A", Address = "X" };
            _mapperMock.Setup(m => m.Map<Customer>(updateDto)).Returns(customer);
            _repoMock.Setup(r => r.UpdateAsync(It.Is<Customer>(c => c.Id == 1))).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateCustomerAsync(1, updateDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsFalse_WhenUpdateFails()
        {
            // Arrange
            var updateDto = new UpdateCustomerDto { Name = "A", Address = "X" };
            var customer = new Customer { Id = 1, Name = "A", Address = "X" };
            _mapperMock.Setup(m => m.Map<Customer>(updateDto)).Returns(customer);
            _repoMock.Setup(r => r.UpdateAsync(It.Is<Customer>(c => c.Id == 1))).ReturnsAsync(false);

            // Act
            var result = await _service.UpdateCustomerAsync(1, updateDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ReturnsTrue_WhenDeleteSucceeds()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteCustomerAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ReturnsFalse_WhenDeleteFails()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteCustomerAsync(1);

            // Assert
            Assert.False(result);
        }
    }
}
