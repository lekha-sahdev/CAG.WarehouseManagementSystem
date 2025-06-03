public class ApiDispatcher : IApiDispatcher
{
	private readonly HttpClient _httpClient;
	private readonly ILogger<ApiDispatcher> _logger;

	public ApiDispatcher(HttpClient httpClient, ILogger<ApiDispatcher> logger)
	{
		_httpClient = httpClient;
		_logger = logger;
	}

	public async Task DispatchAsync<T>(T dto, string url)
	{
		try
		{
			var response = await _httpClient.PostAsJsonAsync(url, dto);
			response.EnsureSuccessStatusCode();
			_logger.LogInformation("Successfully posted {Type} to {Url}", typeof(T).Name, url);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to POST {Type} to {Url}", typeof(T).Name, url);
			throw;
		}
	}
}
