namespace CAG.WarehouseManagementSystem.Dtos
{
	public class OrderDto
	{
		public required int Id { get; set; }
		public required int ProductId { get; set; }
		public required int Quantity { get; set; }

	}
}
