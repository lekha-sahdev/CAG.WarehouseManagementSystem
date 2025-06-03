using System.Xml.Serialization;

namespace CAG.LegacyWareHouseMngntSystem.Dtos
{
	[XmlRoot("Root")]
	public class ProductDto
	{
		public required string ProductCode { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; }
		public required string Dimensions { get; set; }

	}
}
