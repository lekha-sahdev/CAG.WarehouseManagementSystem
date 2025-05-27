using System.ComponentModel.DataAnnotations;

namespace CAG.WarehouseManagementSystem.Data.Entities
{
	public class Product
	{
		[Key]
		public int ProductId { get; set; }

		[Required]
		[MaxLength(50)]
		public required string ProductCode { get; set; }

		[Required]
		[MaxLength(100)]
		public required string Title { get; set; }

		[MaxLength(500)]
		public string? Description { get; set; }

		[MaxLength(50)]
		[Required]
		public required string Dimensions { get; set; }

		// Add other properties as needed
	}
}