namespace CAG.WarehouseManagementSystem.Dtos
{
	public class PurchaseOrderDto
	{
		public required int Id { get; set; }
		public required DateTime ProcessingDate { get; set; }
		public required int CustomerId { get; set; }
		public required List<OrderDto> OrdersDto { get; set; } = new List<OrderDto>();

	}
}
