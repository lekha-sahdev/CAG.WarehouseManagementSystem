using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAG.WarehouseManagementSystem.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class PurchaseOrderController(IPurchaseOrderService PurchaseOrderService, ILogger<PurchaseOrderController> logger) : ControllerBase
	{
		private readonly IPurchaseOrderService _PurchaseOrderService = PurchaseOrderService;
		private readonly ILogger<PurchaseOrderController> _logger = logger;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetPurchaseOrders()
		{
			var PurchaseOrders = await _PurchaseOrderService.GetAllPurchaseOrdersAsync();
			return Ok(PurchaseOrders);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<PurchaseOrderDto>> GetPurchaseOrder(int id)
		{
			_logger.LogInformation("Fetching Purchase Order with ID: {Id}", id);
			var purchaseOrder = await _PurchaseOrderService.GetPurchaseOrderByIdAsync(id);
			_logger.LogInformation($"Product with ID: {id} found - {purchaseOrder == null}");
			return purchaseOrder == null ? NoContent() : Ok(purchaseOrder);
		}

		[HttpPost]
		public async Task<ActionResult<PurchaseOrderDto>> PostPurchaseOrder(PurchaseOrderDto PurchaseOrderDto)
		{
			var createdPurchaseOrder = await _PurchaseOrderService.CreatePurchaseOrderAsync(PurchaseOrderDto);
			_logger.LogInformation($"Purchase Order created with ID: {createdPurchaseOrder.Id}");
			return CreatedAtAction(nameof(GetPurchaseOrder), new { id = createdPurchaseOrder.Id }, createdPurchaseOrder);
		}
	}
}
