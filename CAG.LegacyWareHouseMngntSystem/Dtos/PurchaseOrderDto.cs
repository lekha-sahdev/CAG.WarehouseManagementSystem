using System.Xml.Serialization;

namespace CAG.LegacyWareHouseMngntSystem.Dtos
{
	[XmlRoot("Root")]
	public class PurchaseOrderDto
	{
		public required int Id { get; set; }
		public required DateTime ProcessingDate { get; set; }
		public required int CustomerId { get; set; }

		[XmlArray("OrdersDto")]
		[XmlArrayItem("Item")]
		public required List<OrderDto> OrdersDto { get; set; } = new List<OrderDto>();

	}
}
