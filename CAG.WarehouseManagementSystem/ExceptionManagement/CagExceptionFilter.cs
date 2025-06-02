using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CAG.WarehouseManagementSystem.ExceptionManagement
{
	public class CagExceptionFilter(ILogger<CagExceptionFilter> logger) : IExceptionFilter

	{
		private const string MESSAGE = "An internal server error occured. Please contact Support";
		private const string BAD_DATA_MESSAGE = "Invalid Data. Please re-check the request";

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
			else if (context.Exception is DbUpdateException dbUpdateEx)
			{
				var message = ExtractForeignKeyErrorDetails(dbUpdateEx);

				logger.LogError(dbUpdateEx, "Database update error: {Message}", message);

				ErrorEntity ee = new ErrorEntity(BAD_DATA_MESSAGE, message);
				context.Result = new ObjectResult(ee)
				{
					StatusCode = (int?)HttpStatusCode.BadRequest
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
		private string ExtractForeignKeyErrorDetails(DbUpdateException dbEx)
		{
			try
			{
				var entries = dbEx.Entries;

				if (entries == null || !entries.Any())
					return dbEx.Message;

				var details = entries.Select(entry =>
				{
					var entityType = entry.Entity.GetType().Name;
					var entityTypeMetadata = entry.Context.Model?.FindEntityType(entry.Entity.GetType());

					if (entityTypeMetadata == null)
						return $"Entity: {entityType}, Foreign Keys: Unable to retrieve metadata";

					var foreignKeys = entityTypeMetadata.GetForeignKeys();

					var fkInfo = foreignKeys.Select(fk =>
					{
						var props = fk.Properties.Select(p => p.Name).ToList();
						var propValues = props.Select(propName =>
						{
							var val = entry.Property(propName)?.CurrentValue;
							return $"{propName}={val ?? "null"}";
						});

						return $"FK ({string.Join(", ", propValues)}) references {fk.PrincipalEntityType.Name}";
					});

					return $"Entity: {entityType}, Foreign Keys: {string.Join("; ", fkInfo)}";
				});

				return string.Join("\n", details);
			}
			catch
			{
				// fallback message
				return $"Foreign key error: {dbEx.Message}";
			}
		}
	}
}
