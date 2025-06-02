namespace CAG.WarehouseManagementSystem.Dtos
{
	public class UpdateProductDto
	{
		public required string Title { get; set; }
		public string? Description { get; set; }
		public required string Dimensions { get; set; }

	}
}
