namespace CAG.WarehouseManagementSystem.ExceptionManagement
{
	public class ErrorEntity(string message, string detailedErrorMessage)
	{
		public string message { get; set; } = message;
		public string detailedErrorMessage { get; set; } = detailedErrorMessage;
	}
}
