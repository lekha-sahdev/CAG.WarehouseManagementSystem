using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CAG.WarehouseManagementSystem.ExceptionManagement
{
	public class CagExceptionFilter(ILogger<CagExceptionFilter> logger) : IExceptionFilter

	{
		private const string MESSAGE = "An internal server error occured. Please contact Support";

		private readonly ILogger<CagExceptionFilter> logger = logger;
		public void OnException(ExceptionContext context)
		{
			logger.LogError(context.Exception, MESSAGE);

			if (context.Exception is CagBusinessException exception)
			{
				ErrorEntity ee = new ErrorEntity(exception.message, exception.detailedErrorMessage);
				context.Result = new ObjectResult(ee)
				{
					StatusCode = (int)(exception.statusCode != null ? exception.statusCode : HttpStatusCode.InternalServerError)
				};
			}
			else if (context.Exception is HttpRequestException httpRequestException)
			{
				ErrorEntity ee = new ErrorEntity(MESSAGE, httpRequestException.Message);
				context.Result = new ObjectResult(ee)
				{
					StatusCode = (int?)httpRequestException.StatusCode
				};

			}
			else
			{
				ErrorEntity ee = new ErrorEntity(MESSAGE, context.Exception.Message);
				context.Result = new ObjectResult(ee)
				{
					StatusCode = (int?)HttpStatusCode.InternalServerError
				};

			}

			context.ExceptionHandled = true;
		}
	}
}
