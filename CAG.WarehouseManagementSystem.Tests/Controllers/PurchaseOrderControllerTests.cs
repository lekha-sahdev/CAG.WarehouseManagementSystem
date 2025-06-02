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
    public class PurchaseOrderControllerTests
    {
        private readonly Mock<IPurchaseOrderService> _serviceMock;
        private readonly Mock<ILogger<PurchaseOrderController>> _loggerMock;
        private readonly PurchaseOrderController _controller;

        public PurchaseOrderControllerTests()
        {
            _serviceMock = new Mock<IPurchaseOrderService>();
            _loggerMock = new Mock<ILogger<PurchaseOrderController>>();
            _controller = new PurchaseOrderController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetPurchaseOrders_ReturnsOk_WithPurchaseOrderList()
        {
            // Arrange
            List<OrderDto> ordersDto = new List<OrderDto>();
            ordersDto.Add(new OrderDto { Id = 1, ProductId = 1, Quantity = 10 });
            var orders = new List<PurchaseOrderDto>
            {
                new PurchaseOrderDto { Id = 1, CustomerId = 1, ProcessingDate = DateTime.Now, OrdersDto = ordersDto},
                new PurchaseOrderDto { Id = 2, CustomerId = 2, ProcessingDate = DateTime.Now, OrdersDto = ordersDto }
            };
            _serviceMock.Setup(s => s.GetAllPurchaseOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetPurchaseOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PurchaseOrderDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetPurchaseOrder_ReturnsOk_WhenOrderExists()
        {
			// Arrange
			List<OrderDto> ordersDto = new List<OrderDto>();
			ordersDto.Add(new OrderDto { Id = 1, ProductId = 1, Quantity = 10 });
			var order = new PurchaseOrderDto { Id = 1, CustomerId = 1, ProcessingDate = DateTime.Now, OrdersDto = ordersDto };
            _serviceMock.Setup(s => s.GetPurchaseOrderByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetPurchaseOrder(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PurchaseOrderDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task PostPurchaseOrder_ReturnsCreatedAtAction()
        {
			// Arrange
			List<OrderDto> ordersDto = new List<OrderDto>();
			ordersDto.Add(new OrderDto { Id = 1, ProductId = 1, Quantity = 10 });
			var orderDto = new PurchaseOrderDto { Id = 1, CustomerId = 1 , ProcessingDate = DateTime.Now, OrdersDto = ordersDto };
			_serviceMock.Setup(s => s.CreatePurchaseOrderAsync(orderDto)).ReturnsAsync(orderDto);

            // Act
            var result = await _controller.PostPurchaseOrder(orderDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetPurchaseOrder", createdAtActionResult.ActionName);
            Assert.Equal(orderDto.Id, ((PurchaseOrderDto)createdAtActionResult.Value!).Id);
        }
    }
}
