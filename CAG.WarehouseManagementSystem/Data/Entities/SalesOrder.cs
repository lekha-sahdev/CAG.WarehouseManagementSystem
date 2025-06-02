using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAG.WarehouseManagementSystem.Data.Entities
{
	public class SalesOrder
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime ProcessingDate { get; set; }

		[Required]
		public int CustomerId { get; set; }

		[ForeignKey("CustomerId")]
		public Customer Customer { get; set; } 

		public required ICollection<SalesOrderProduct> SalesOrderProducts { get; set; }

		[Required]
		public required string ShipmentAddress { get; set; }
	}
}