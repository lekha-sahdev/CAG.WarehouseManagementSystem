using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAG.WarehouseManagementSystem.Data.Entities
{
	public class PurchaseOrder
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime ProcessingDate { get; set; }

		[Required]
		public int CustomerId { get; set; }

		[ForeignKey(nameof(CustomerId))]
		public Customer Customer { get; set; }

		public required ICollection<PurchaseOrderProduct> PurchaseOrderProducts { get; set; }
	}
}