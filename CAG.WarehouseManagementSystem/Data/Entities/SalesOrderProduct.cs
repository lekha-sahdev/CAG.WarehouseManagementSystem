using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAG.WarehouseManagementSystem.Data.Entities
{
	public class SalesOrderProduct
	{
		public int Id { get; set; }

		[Required]
		public int SalesOrderId { get; set; }

		[ForeignKey("SalesOrderId")]
		public SalesOrder SalesOrder { get; set; }

		[Required]
		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		public Product Product { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}