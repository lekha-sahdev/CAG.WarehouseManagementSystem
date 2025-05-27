using System.Net;

namespace CAG.WarehouseManagementSystem.ExceptionManagement
{
	public class CagBusinessException : Exception
	{
		public HttpStatusCode? statusCode { get; private set; }
		public string message { get; private set; }
		public string detailedErrorMessage { get; private set; }

		public CagBusinessException(string message, string detailedErrorMessage, HttpStatusCode? code = null) : base(detailedErrorMessage)
		{
			this.statusCode = code;
			this.message = message;
			this.detailedErrorMessage = detailedErrorMessage;
		}

		public CagBusinessException(string message, string detailedErrorMessage, Exception ex, HttpStatusCode? code = null)
			: base(detailedErrorMessage, ex)
		{
			this.statusCode = code;
			this.message = message;
			this.detailedErrorMessage = detailedErrorMessage;
		}
	}
}
