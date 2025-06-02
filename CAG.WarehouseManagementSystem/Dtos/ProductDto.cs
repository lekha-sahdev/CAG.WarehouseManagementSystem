namespace CAG.WarehouseManagementSystem.Dtos
{
	public class ProductDto
	{
		public int Id { get; private set; }
		public required string ProductCode { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; }
		public required string Dimensions { get; set; }

	}
}
