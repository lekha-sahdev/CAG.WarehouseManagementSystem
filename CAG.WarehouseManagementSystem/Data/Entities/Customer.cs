namespace CAG.WarehouseManagementSystem.Data.Entities
{
	public class Customer
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Address { get; set; }
	}
}
