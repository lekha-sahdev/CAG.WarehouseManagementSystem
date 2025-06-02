using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAG.WarehouseManagementSystem.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class SalesOrderController(ISalesOrderService SalesOrderService, ILogger<SalesOrderController> logger) : ControllerBase
	{
		private readonly ISalesOrderService _SalesOrderService = SalesOrderService;
		private readonly ILogger<SalesOrderController> _logger = logger;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<SalesOrderDto>>> GetSalesOrders()
		{
			var SalesOrders = await _SalesOrderService.GetAllSalesOrdersAsync();
			return Ok(SalesOrders);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<SalesOrderDto>> GetSalesOrder(int id)
		{
			_logger.LogInformation("Fetching Sales Order with ID: {Id}", id);
			var salesOrder = await _SalesOrderService.GetSalesOrderByIdAsync(id);
			_logger.LogInformation($"Product with ID: {id} found - {salesOrder == null}");
			return salesOrder == null ? NoContent() : Ok(salesOrder);
		}

		[HttpPost]
		public async Task<ActionResult<SalesOrderDto>> PostSalesOrder(SalesOrderDto SalesOrderDto)
		{
			var createdSalesOrder = await _SalesOrderService.CreateSalesOrderAsync(SalesOrderDto);
			_logger.LogInformation($"Sales Order created with ID: {createdSalesOrder.Id}");
			return CreatedAtAction(nameof(GetSalesOrder), new { id = createdSalesOrder.Id }, createdSalesOrder);
		}
	}
}
