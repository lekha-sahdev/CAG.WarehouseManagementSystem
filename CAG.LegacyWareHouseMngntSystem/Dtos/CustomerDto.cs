using System.Xml.Serialization;

namespace CAG.LegacyWareHouseMngntSystem.Dtos
{
	[XmlRoot("Root")]
	public class CustomerDto
	{
		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
	}

}
