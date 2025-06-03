using Autofac.Features.Indexed;
using CAG.LegacyWareHouseMngntSystem.Dtos;
using Polly;

public class FileProcessService : IFileProcessService
{
	private readonly IIndex<string, IFileParser> _parsers;
	private readonly IApiDispatcher _dispatcher;
	private readonly ILogger<FileProcessService> _logger;
	private readonly Dictionary<string, (string Url, Func<string, Task<object?>> Parse)> _map;
	private readonly IConfiguration _configuration;
	private readonly AsyncPolicy _retryPolicy;
	private readonly string? archivePath;

	public FileProcessService(IIndex<string, IFileParser> parsers, IApiDispatcher dispatcher, ILogger<FileProcessService> logger, 
		IConfiguration configuration, AsyncPolicy asyncPolicy)
	{
		_parsers = parsers;
		_dispatcher = dispatcher;
		_logger = logger;
		_configuration = configuration;
		_retryPolicy = asyncPolicy;
		archivePath = _configuration.GetValue<string>("Polling:ArchivePath");
		_map = new Dictionary<string, (string, Func<string, Task<object?>>)>
		{
			{ "Customer", ("CreateCustomerUrl", async filePath => await ParseFactory<CustomerDto>(filePath)) },
			{ "Product", ("CreateProductUrl", async filePath => await ParseFactory<ProductDto>(filePath)) },
			{ "SalesOrder", ("CreateSalesOrderUrl", async filePath => await ParseFactory<SalesOrderDto>(filePath)) },
			{ "PurchaseOrder", ("CreatePurchaseOrderUrl", async filePath => await ParseFactory<PurchaseOrderDto>(filePath)) }
		};		
	}

	public async Task ProcessFileAsync(string filePath)
	{
		try
		{
			string fileName = Path.GetFileName(filePath); // "Customer_2025.pdf"
			string entityName = fileName.Split('_')[0];
			_map.TryGetValue(entityName, out var endpointInfo);
			string? endpointUrl = _configuration.GetValue<string>(endpointInfo.Url);
			validateEntityName(entityName, endpointInfo, endpointUrl);
			var data = await endpointInfo.Parse(filePath);
			if (data == null)
			{
				_logger.LogWarning("File {FilePath} could not be deserialized to type", filePath);
				return;
			}

			await _dispatcher.DispatchAsync(data, endpointUrl);
			
		}
		catch (Exception ex)
		{
			_logger.LogError($"error occured processing {filePath}", ex); //Not throwing this error to let the job continue next instance
		}
		finally
		{
			await ArchiveFile(filePath);
			_logger.LogInformation("Processed file: {FilePath}", filePath);
		}
		
	}

	private void validateEntityName(string entityName, (string Url, Func<string, Task<object?>> Parse) endpointInfo, string? url)
	{
		if (endpointInfo == default)
			throw new ArgumentException($"No endpoint configured for entity '{entityName}'");
		string endpointUrl = url ??
			throw new ArgumentException($"No endpoint configured for entity '{entityName}'");
	}

	private async Task<T?> ParseFactory<T>(string filePath) where T : class
	{
		string extension = Path.GetExtension(filePath);
		if (_parsers.TryGetValue(extension.ToLowerInvariant().TrimStart('.'), out var parser))
		{
			return await _retryPolicy.ExecuteAsync(async () =>
			{
				 return await parser.Parse<T>(filePath);
			});
		}
		else
		{
			throw new ArgumentException($"No parser found for type '{typeof(T).Name}'");
		}
	}

	private async Task ArchiveFile(string filePath)
	{
		try
		{
			Directory.CreateDirectory(archivePath);

			var fileName = Path.GetFileName(filePath);
			var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff");
			var archiveFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}{Path.GetExtension(fileName)}";
			var destPath = Path.Combine(archivePath, archiveFileName);

			File.Move(filePath, destPath);

			_logger.LogInformation("Archived file to {Path}", destPath);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to archive file: {Path}", filePath);
		}

		await Task.CompletedTask; // just to keep method async-compatible
	}

}
