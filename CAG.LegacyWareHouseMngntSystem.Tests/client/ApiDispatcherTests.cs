using Xunit;
using Moq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using Moq.Protected;

public class ApiDispatcherTests
{
	private readonly HttpClient _httpClient;
	private readonly Mock<ILogger<ApiDispatcher>> _loggerMock;
    private readonly ApiDispatcher _dispatcher;
	private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

	public ApiDispatcherTests()
    {
		_httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
		_httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _loggerMock = new Mock<ILogger<ApiDispatcher>>();
        _dispatcher = new ApiDispatcher(_httpClient, _loggerMock.Object);
    }

    [Fact]
    public async Task DispatchAsync_SuccessfulPost_LogsInformation()
    {
        // Arrange
        var dto = new { Name = "Test" };
        var url = "https://api/test";
		_httpMessageHandlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)) // or BadRequest for failure test
			.Verifiable();

		// Act
		await _dispatcher.DispatchAsync(dto, url);
		// Assert
		_httpMessageHandlerMock.Verify();
	}
	[Fact]
	public async Task DispatchAsync_FailedPost_LogsErrorAndThrows()
	{
		// Arrange
		var dto = new { Name = "Test" };
		var url = "https://api/test";
		_httpMessageHandlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

		// Act
		await Assert.ThrowsAsync<HttpRequestException>(() => _dispatcher.DispatchAsync(dto, url));
    }

}
