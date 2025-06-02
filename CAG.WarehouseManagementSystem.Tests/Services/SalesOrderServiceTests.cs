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
    public class SalesOrderServiceTests
    {
        private readonly Mock<ISalesOrderRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SalesOrderService _service;

        public SalesOrderServiceTests()
        {
            _repoMock = new Mock<ISalesOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new SalesOrderService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllSalesOrdersAsync_ReturnsMappedDtos()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer", Address = "USA" };
            List<SalesOrderProduct> salesOrderProducts = new List<SalesOrderProduct>
			{
				new SalesOrderProduct { Id = 1, ProductId = 1, Quantity = 5, SalesOrderId = 2 }
            };

            var salesOrders = new List<SalesOrder> {
                new SalesOrder {Id = 1, ShipmentAddress = "UAE", ProcessingDate = DateTime.Now, CustomerId =1,
                    SalesOrderProducts = salesOrderProducts },
                new SalesOrder {Id = 2, ShipmentAddress = "UAE", ProcessingDate = DateTime.Now, CustomerId =2,
                    SalesOrderProducts = salesOrderProducts} };
            List<OrderDto> ordersDto = new List<OrderDto> { new OrderDto { Id = 1, ProductId = 1, Quantity = 5} };
            var salesOrderDtos = new List<SalesOrderDto> {
                new SalesOrderDto { Id = 1, ShipmentAddress = "UAE", ProcessingDate = DateTime.Now, CustomerId = 1, OrdersDto = ordersDto},
                new SalesOrderDto { Id = 2 , ShipmentAddress = "UAE", ProcessingDate = DateTime.Now, CustomerId = 1, OrdersDto = ordersDto}
            };
            _repoMock.Setup(r => r.GetAllAsyncWithOrders()).ReturnsAsync(salesOrders);
            _mapperMock.Setup(m => m.Map<IEnumerable<SalesOrderDto>>(salesOrders)).Returns(salesOrderDtos);

            // Act
            var result = await _service.GetAllSalesOrdersAsync();

            // Assert
            Assert.Equal(2, ((List<SalesOrderDto>)result).Count);
        }

        [Fact]
        public async Task GetSalesOrderByIdAsync_ReturnsMappedDto_WhenFound()
        {
			// Arrange
			var customer = new Customer { Id = 1, Name = "Test Customer", Address = "USA" };
			List<SalesOrderProduct> salesOrderProducts = new List<SalesOrderProduct>
			{
				new SalesOrderProduct { Id = 1, ProductId = 1, Quantity = 5, SalesOrderId = 1 }
			};

            var salesOrder =
                new SalesOrder
                {
                    Id = 1,
                    ShipmentAddress = "UAE",
                    ProcessingDate = DateTime.Now,
                    CustomerId = 1,
                    SalesOrderProducts = salesOrderProducts
                };
			List<OrderDto> ordersDto = new List<OrderDto> { new OrderDto { Id = 1, ProductId = 1, Quantity = 5 } };
            var salesOrderDto = new SalesOrderDto { Id = 1, ShipmentAddress = "UAE", ProcessingDate = DateTime.Now, CustomerId = 1, OrdersDto = ordersDto };
			_repoMock.Setup(r => r.GetByIdAsyncWithOrders(1)).ReturnsAsync(salesOrder);
            _mapperMock.Setup(m => m.Map<SalesOrderDto>(salesOrder)).Returns(salesOrderDto);

            // Act
            var result = await _service.GetSalesOrderByIdAsync(1);

            // Assert
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetSalesOrderByIdAsync_Throws_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsyncWithOrders(99)).ReturnsAsync((SalesOrder)null);

            // Act & Assert
            await Assert.ThrowsAsync<CagBusinessException>(() => _service.GetSalesOrderByIdAsync(99));
        }

        [Fact]
        public async Task CreateSalesOrderAsync_ReturnsMappedDto()
        {
			// Arrange
			var customer = new Customer { Id = 1, Name = "Test Customer", Address = "USA" };
			List<SalesOrderProduct> salesOrderProducts = new List<SalesOrderProduct>
			{
				new SalesOrderProduct { Id = 1, ProductId = 1, Quantity = 5, SalesOrderId = 1 }
			};

			var salesOrder =
				new SalesOrder {Id = 1, ShipmentAddress = "UAE", ProcessingDate = DateTime.Now, CustomerId =1,
					SalesOrderProducts = salesOrderProducts } ;
			List<OrderDto> ordersDto = new List<OrderDto> { new OrderDto { Id = 1, ProductId = 1, Quantity = 5 } };
            var salesOrderDto =
                new SalesOrderDto { Id = 1, ShipmentAddress = "UAE", ProcessingDate = DateTime.Now, CustomerId = 1, OrdersDto = ordersDto };

			_mapperMock.Setup(m => m.Map<SalesOrder>(salesOrderDto)).Returns(salesOrder);
            _repoMock.Setup(r => r.AddAsync(salesOrder)).ReturnsAsync(salesOrder);
            _mapperMock.Setup(m => m.Map<SalesOrderDto>(salesOrder)).Returns(salesOrderDto);

            // Act
            var result = await _service.CreateSalesOrderAsync(salesOrderDto);

            // Assert
            Assert.Equal(1, result.Id);
        }
    }
}
