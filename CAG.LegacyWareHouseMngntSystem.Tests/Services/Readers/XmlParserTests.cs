using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class XmlParserTests
{
    private readonly Mock<ILogger<XmlParser>> _loggerMock;
    private readonly XmlParser _parser;

    public XmlParserTests()
    {
        _loggerMock = new Mock<ILogger<XmlParser>>();
        _parser = new XmlParser(_loggerMock.Object);
    }

    [Fact]
    public async Task Parse_ValidXml_ReturnsObject()
    {
        // Arrange
        var filePath = "customer.xml";
        var xml = "<CustomerDto><Name>John</Name><Address>123 Main</Address></CustomerDto>";
        await File.WriteAllTextAsync(filePath, xml);

        // Act
        var result = await _parser.Parse<CustomerDto>(filePath);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.Name);
        Assert.Equal("123 Main", result.Address);

        // Cleanup
        File.Delete(filePath);
    }

    [Fact]
    public async Task Parse_InvalidXml_LogsErrorAndReturnsNull()
    {
        // Arrange
        var filePath = "invalid.xml";
        await File.WriteAllTextAsync(filePath, "<not valid xml>");

        // Act
        var result = await _parser.Parse<CustomerDto>(filePath);

        // Assert
        Assert.Null(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to read or deserialize XML file")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

        // Cleanup
        File.Delete(filePath);
    }

    [Fact]
    public async Task Parse_FileNotFound_LogsErrorAndReturnsNull()
    {
        // Arrange
        var filePath = "doesnotexist.xml";

        // Act
        var result = await _parser.Parse<CustomerDto>(filePath);

        // Assert
        Assert.Null(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to read or deserialize XML file")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}

// Example DTO for testing
public class CustomerDto
{
    public string Name { get; set; }
    public string Address { get; set; }
}
