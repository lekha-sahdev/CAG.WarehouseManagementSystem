using CAG.WarehouseManagementSystem.Controllers;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.ExceptionManagement;
using CAG.WarehouseManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CAG.WarehouseManagementSystem.Tests
{
	public class CustomerControllerTests
	{
		private readonly Mock<ICustomerService> _customerServiceMock;
		private readonly CustomerController _customerController;

		public CustomerControllerTests()
		{
			_customerServiceMock = new Mock<ICustomerService>();
			var loggerMock = new Mock<ILogger<CustomerController>>();
			_customerController = new CustomerController(_customerServiceMock.Object, loggerMock.Object);
		}

		[Fact]
		public async Task GetCustomers_ShouldReturnOkResult_WithListOfCustomers()
		{
			// Arrange
			var customers = new List<CustomerDto>
			{
				new CustomerDto { CustomerId = 1, Name = "John Doe", Address = "123 Main St" },
				new CustomerDto { CustomerId = 2, Name = "Jane Doe", Address = "456 Elm St" }
			};
			_customerServiceMock.Setup(service => service.GetAllCustomersAsync()).ReturnsAsync(customers);

			// Act
			var result = await _customerController.GetCustomers();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<List<CustomerDto>>(okResult.Value);
			Assert.Equal(2, returnValue.Count);
		}

		[Fact]
		public async Task GetCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
		{
			// Arrange
			_customerServiceMock.Setup(service => service.GetCustomerByIdAsync(10))
				.Throws<CagBusinessException>(() => new CagBusinessException("Not Found", "Not Found"));

			// Act
			var ex = await Assert.ThrowsAsync<CagBusinessException>(() => _customerController.GetCustomer(10));

			// Assert
			Assert.Equal(ex.Message, "Not Found");
		}

		[Fact]
		public async Task PostCustomer_ShouldReturnCreatedAtActionResult()
		{
			// Arrange
			var customerDto = new CustomerDto { Name = "John Doe", Address = "123 Main St" };
			_customerServiceMock.Setup(service => service.CreateCustomerAsync(customerDto)).ReturnsAsync(customerDto);

			// Act
			var result = await _customerController.PostCustomer(customerDto);

			// Assert
			var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
			Assert.Equal("GetCustomer", createdAtActionResult.ActionName);
		}

		[Fact]
		public async Task PutCustomer_ShouldReturnNoContent_WhenUpdateIsSuccessful()
		{
			// Arrange
			var customerDto = new UpdateCustomerDto { Name = "John Doe", Address = "123 Main St" };
			_customerServiceMock.Setup(service => service.UpdateCustomerAsync(1, customerDto)).ReturnsAsync(true);

			// Act
			var result = await _customerController.PutCustomer(1, customerDto);

			// Assert
			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task DeleteCustomer_ShouldReturnNoContent_WhenDeletionIsSuccessful()
		{
			// Arrange
			_customerServiceMock.Setup(service => service.DeleteCustomerAsync(1)).ReturnsAsync(true);

			// Act
			var result = await _customerController.DeleteCustomer(1);

			// Assert
			Assert.IsType<NoContentResult>(result);
		}
	}
}