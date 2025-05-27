using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAG.WarehouseManagementSystem.Data.Entities
{
	public class PurchaseOrder
	{
		[Key]
		public int OrderId { get; set; }

		[Required]
		public DateTime ProcessingDate { get; set; }

		[Required]
		public int CustomerId { get; set; }

		[ForeignKey("CustomerId")]
		public required Customer Customer { get; set; }

		public required ICollection<PurchaseOrderProduct> PurchaseOrderProducts { get; set; }
	}
}