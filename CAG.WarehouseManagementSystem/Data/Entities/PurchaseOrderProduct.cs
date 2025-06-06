using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAG.WarehouseManagementSystem.Data.Entities
{
	public class PurchaseOrderProduct
	{
		[Key]
		public int Id { get; set; }

		public int PurchaseOrderId { get; set; }

		[ForeignKey("PurchaseOrderId")]
		public PurchaseOrder PurchaseOrder { get; set; }

		[Required]
		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		public Product Product { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}