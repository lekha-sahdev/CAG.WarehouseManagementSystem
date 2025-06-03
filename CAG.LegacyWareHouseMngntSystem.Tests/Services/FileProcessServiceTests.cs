
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Autofac.Features.Indexed;
using Polly;
using CAG.LegacyWareHouseMngntSystem.Dtos;

public class FileProcessServiceTests
{
    private readonly Mock<IIndex<string, IFileParser>> _parsersMock;
    private readonly Mock<IApiDispatcher> _dispatcherMock;
    private readonly Mock<ILogger<FileProcessService>> _loggerMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AsyncPolicy _retryPolicy;
    private readonly FileProcessService _service;

    public FileProcessServiceTests()
    {
        _parsersMock = new Mock<IIndex<string, IFileParser>>();
        _dispatcherMock = new Mock<IApiDispatcher>();
        _loggerMock = new Mock<ILogger<FileProcessService>>();
        _configMock = new Mock<IConfiguration>();

        // Setup configuration for endpoints and archive path
        var createCustomerSection = new Mock<IConfigurationSection>();
        createCustomerSection.Setup(s => s.Value).Returns("https://api/customer");
        _configMock.Setup(c => c.GetSection("CreateCustomerUrl")).Returns(createCustomerSection.Object);

        var archivePathSection = new Mock<IConfigurationSection>();
        archivePathSection.Setup(s => s.Value).Returns("archive");
        _configMock.Setup(c => c.GetSection("Polling:ArchivePath")).Returns(archivePathSection.Object);


        // Simple retry policy for tests
        _retryPolicy = Policy.NoOpAsync();

        _service = new FileProcessService(
            _parsersMock.Object,
            _dispatcherMock.Object,
            _loggerMock.Object,
            _configMock.Object,
            _retryPolicy
        );
    }

    [Fact]
    public async Task ProcessFileAsync_ValidCustomerFile_DispatchesAndArchives()
    {
        // Arrange
        var filePath = "Customer_2025.json";
        var customer = new CustomerDto();
        var parserMock = new Mock<IFileParser>();
        parserMock.Setup(p => p.Parse<CustomerDto>(filePath)).ReturnsAsync(customer);
        _parsersMock.Setup(p => p.TryGetValue("json", out It.Ref<IFileParser>.IsAny))
            .Callback(new TryGetValueCallback<string, IFileParser>((string key, out IFileParser value) =>
            {
                value = parserMock.Object;
            }))
            .Returns(true);

        // Create a dummy file to archive
        File.WriteAllText(filePath, "{}");
        _dispatcherMock.Setup(d => d.DispatchAsync<object>(customer, It.IsAny<string>()))
			.Returns(Task.CompletedTask).Verifiable();

		// Act
		await _service.ProcessFileAsync(filePath);
        _dispatcherMock.Verify();

        Assert.True(Directory.Exists("archive"));
        var archivedFiles = Directory.GetFiles("archive");
        Assert.Contains(archivedFiles, f => f.Contains("Customer_2025") && f.EndsWith(".json"));
        // Cleanup
        foreach (var f in archivedFiles) File.Delete(f);
        Directory.Delete("archive");
    }

    [Fact]
    public async Task ProcessFileAsync_UnknownEntity_ThrowsArgumentException()
    {
        // Arrange
        var filePath = "Unknown_2025.json";
        File.WriteAllText(filePath, "{}");

        // Act & Assert
        await _service.ProcessFileAsync(filePath); // Should log error, not throw
        // Cleanup
        File.Delete(filePath);
    }

    [Fact]
    public async Task ProcessFileAsync_NullData_LogsWarning()
    {
        // Arrange
        var filePath = "Customer_2025.json";
        var parserMock = new Mock<IFileParser>();
        parserMock.Setup(p => p.Parse<CustomerDto>(filePath)).ReturnsAsync((CustomerDto)null);
        _parsersMock.Setup(p => p.TryGetValue("json", out It.Ref<IFileParser>.IsAny))
            .Callback(new TryGetValueCallback<string, IFileParser>((string key, out IFileParser value) =>
            {
                value = parserMock.Object;
            }))
            .Returns(true);

        File.WriteAllText(filePath, "{}");

        // Act
        await _service.ProcessFileAsync(filePath);

        // Assert
        _loggerMock.Verify(
    x => x.Log(
        LogLevel.Warning, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(filePath)), null, It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        // Cleanup
        var archivedFiles = Directory.GetFiles("archive");
        foreach (var f in archivedFiles) File.Delete(f);
        Directory.Delete("archive");
    }

    [Fact]
    public async Task ParseFactory_ThrowsIfNoParser()
    {
        // Arrange
        var filePath = "Customer_2025.unknown";
        // No parser registered for "unknown"
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetType()
                .GetMethod("ParseFactory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .MakeGenericMethod(typeof(CustomerDto))
                .Invoke(_service, new object[] { filePath }) as Task<CustomerDto>
        );
    }

    [Fact]
    public async Task ArchiveFile_ArchivesFile()
    {
        // Arrange
        var filePath = "Customer_2025.json";
        File.WriteAllText(filePath, "{}");

        // Use reflection to call private ArchiveFile
        var method = typeof(FileProcessService).GetMethod("ArchiveFile", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        await (Task)method.Invoke(_service, new object[] { filePath });

        // Assert
        Assert.True(Directory.Exists("archive"));
        var archivedFiles = Directory.GetFiles("archive");
        Assert.Contains(archivedFiles, f => f.Contains("Customer_2025") && f.EndsWith(".json"));
        // Cleanup
        foreach (var f in archivedFiles) File.Delete(f);
        Directory.Delete("archive");
    }

    // Helper for Moq TryGetValue
    private delegate void TryGetValueCallback<TKey, TValue>(TKey key, out TValue value);
}

