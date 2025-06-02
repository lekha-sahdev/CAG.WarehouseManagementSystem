using CAG.WarehouseManagementSystem.Controllers;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CAG.WarehouseManagementSystem.Tests.Controllers
{
    public class SalesOrderControllerTests
    {
        private readonly Mock<ISalesOrderService> _serviceMock;
        private readonly Mock<ILogger<SalesOrderController>> _loggerMock;
        private readonly SalesOrderController _controller;

        public SalesOrderControllerTests()
        {
            _serviceMock = new Mock<ISalesOrderService>();
            _loggerMock = new Mock<ILogger<SalesOrderController>>();
            _controller = new SalesOrderController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetSalesOrders_ReturnsOk_WithSalesOrderList()
        {
            // Arrange
            var orderProducts = new List<OrderDto> { new OrderDto { Id = 1, ProductId = 1, Quantity = 5 } };
            var orders = new List<SalesOrderDto>
            {
                new SalesOrderDto { Id = 1, CustomerId = 1, ProcessingDate = DateTime.Now, OrdersDto = orderProducts, ShipmentAddress = "UAE" },
                new SalesOrderDto { Id = 2, CustomerId = 2, ProcessingDate = DateTime.Now, OrdersDto = orderProducts , ShipmentAddress = "UAE" }
			};
            _serviceMock.Setup(s => s.GetAllSalesOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetSalesOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<SalesOrderDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetSalesOrder_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            var orderProducts = new List<OrderDto> { new OrderDto { Id = 1, ProductId = 1, Quantity = 5 } };
            var order = new SalesOrderDto { Id = 1, CustomerId = 1, ProcessingDate = DateTime.Now, OrdersDto = orderProducts, ShipmentAddress = "UAE" };
            _serviceMock.Setup(s => s.GetSalesOrderByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetSalesOrder(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<SalesOrderDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetSalesOrder_ReturnsNoContent_WhenOrderDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetSalesOrderByIdAsync(99)).ReturnsAsync((SalesOrderDto?)null);

            // Act
            var result = await _controller.GetSalesOrder(99);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task PostSalesOrder_ReturnsCreatedAtAction()
        {
            // Arrange
            var orderProducts = new List<OrderDto> { new OrderDto { Id = 1, ProductId = 1, Quantity = 5 } };
            var orderDto = new SalesOrderDto { Id = 1, CustomerId = 1, ProcessingDate = DateTime.Now, OrdersDto = orderProducts, ShipmentAddress = "UAE" };
            _serviceMock.Setup(s => s.CreateSalesOrderAsync(orderDto)).ReturnsAsync(orderDto);

            // Act
            var result = await _controller.PostSalesOrder(orderDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetSalesOrder", createdAtActionResult.ActionName);
            Assert.Equal(orderDto.Id, ((SalesOrderDto)createdAtActionResult.Value!).Id);
        }
    }
}
