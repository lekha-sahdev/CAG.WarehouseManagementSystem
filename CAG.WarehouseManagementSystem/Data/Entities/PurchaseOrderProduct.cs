using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAG.WarehouseManagementSystem.Data.Entities
{
	public class PurchaseOrderProduct
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int PurchaseOrderId { get; set; }

		[ForeignKey("PurchaseOrderId")]
		public required PurchaseOrder PurchaseOrder { get; set; }

		[Required]
		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		public required Product Product { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}