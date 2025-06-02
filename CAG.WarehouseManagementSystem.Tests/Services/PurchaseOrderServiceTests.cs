using AutoMapper;
using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.ExceptionManagement;
using CAG.WarehouseManagementSystem.Services;
using Moq;

namespace CAG.WarehouseManagementSystem.Tests.Services
{
    public class PurchaseOrderServiceTests
    {
        private readonly Mock<IPurchaseOrderRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PurchaseOrderService _service;

        public PurchaseOrderServiceTests()
        {
            _repoMock = new Mock<IPurchaseOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new PurchaseOrderService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllPurchaseOrdersAsync_ReturnsMappedDtos()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer", Address = "USA" };
            List<PurchaseOrderProduct> PurchaseOrderProducts = new List<PurchaseOrderProduct>
			{
				new PurchaseOrderProduct { Id = 1, ProductId = 1, Quantity = 5, PurchaseOrderId = 2 }
            };

            var PurchaseOrders = new List<PurchaseOrder> {
                new PurchaseOrder {Id = 1, ProcessingDate = DateTime.Now, CustomerId =1,
                    PurchaseOrderProducts = PurchaseOrderProducts },
                new PurchaseOrder {Id = 2,  ProcessingDate = DateTime.Now, CustomerId =2,
                    PurchaseOrderProducts = PurchaseOrderProducts} };
            List<OrderDto> ordersDto = new List<OrderDto> { new OrderDto { Id = 1, ProductId = 1, Quantity = 5} };
            var PurchaseOrderDtos = new List<PurchaseOrderDto> {
                new PurchaseOrderDto { Id = 1,  ProcessingDate = DateTime.Now, CustomerId = 1, OrdersDto = ordersDto},
                new PurchaseOrderDto { Id = 2 ,  ProcessingDate = DateTime.Now, CustomerId = 1, OrdersDto = ordersDto}
            };
            _repoMock.Setup(r => r.GetAllAsyncWithOrders()).ReturnsAsync(PurchaseOrders);
            _mapperMock.Setup(m => m.Map<IEnumerable<PurchaseOrderDto>>(PurchaseOrders)).Returns(PurchaseOrderDtos);

            // Act
            var result = await _service.GetAllPurchaseOrdersAsync();

            // Assert
            Assert.Equal(2, ((List<PurchaseOrderDto>)result).Count);
        }

        [Fact]
        public async Task GetPurchaseOrderByIdAsync_ReturnsMappedDto_WhenFound()
        {
			// Arrange
			var customer = new Customer { Id = 1, Name = "Test Customer", Address = "USA" };
			List<PurchaseOrderProduct> PurchaseOrderProducts = new List<PurchaseOrderProduct>
			{
				new PurchaseOrderProduct { Id = 1, ProductId = 1, Quantity = 5, PurchaseOrderId = 1 }
			};

            var PurchaseOrder =
                new PurchaseOrder
                {
                    Id = 1,
                    
                    ProcessingDate = DateTime.Now,
                    CustomerId = 1,
                    PurchaseOrderProducts = PurchaseOrderProducts
                };
			List<OrderDto> ordersDto = new List<OrderDto> { new OrderDto { Id = 1, ProductId = 1, Quantity = 5 } };
            var PurchaseOrderDto = new PurchaseOrderDto { Id = 1,  ProcessingDate = DateTime.Now, CustomerId = 1, OrdersDto = ordersDto };
			_repoMock.Setup(r => r.GetByIdAsyncWithOrders(1)).ReturnsAsync(PurchaseOrder);
            _mapperMock.Setup(m => m.Map<PurchaseOrderDto>(PurchaseOrder)).Returns(PurchaseOrderDto);

            // Act
            var result = await _service.GetPurchaseOrderByIdAsync(1);

            // Assert
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetPurchaseOrderByIdAsync_Throws_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsyncWithOrders(99)).ReturnsAsync((PurchaseOrder)null);

            // Act & Assert
            await Assert.ThrowsAsync<CagBusinessException>(() => _service.GetPurchaseOrderByIdAsync(99));
        }

        [Fact]
        public async Task CreatePurchaseOrderAsync_ReturnsMappedDto()
        {
			// Arrange
			var customer = new Customer { Id = 1, Name = "Test Customer", Address = "USA" };
			List<PurchaseOrderProduct> PurchaseOrderProducts = new List<PurchaseOrderProduct>
			{
				new PurchaseOrderProduct { Id = 1, ProductId = 1, Quantity = 5, PurchaseOrderId = 1 }
			};

			var PurchaseOrder =
				new PurchaseOrder {Id = 1,  ProcessingDate = DateTime.Now, CustomerId =1,
					PurchaseOrderProducts = PurchaseOrderProducts } ;
			List<OrderDto> ordersDto = new List<OrderDto> { new OrderDto { Id = 1, ProductId = 1, Quantity = 5 } };
            var PurchaseOrderDto =
                new PurchaseOrderDto { Id = 1,  ProcessingDate = DateTime.Now, CustomerId = 1, OrdersDto = ordersDto };

			_mapperMock.Setup(m => m.Map<PurchaseOrder>(PurchaseOrderDto)).Returns(PurchaseOrder);
            _repoMock.Setup(r => r.AddAsync(PurchaseOrder)).ReturnsAsync(PurchaseOrder);
            _mapperMock.Setup(m => m.Map<PurchaseOrderDto>(PurchaseOrder)).Returns(PurchaseOrderDto);

            // Act
            var result = await _service.CreatePurchaseOrderAsync(PurchaseOrderDto);

            // Assert
            Assert.Equal(1, result.Id);
        }
    }
}
