using System.Xml.Serialization;

public class XmlParser(ILogger<XmlParser> logger) : IFileParser
{
	private readonly ILogger<XmlParser> _logger = logger;
	public async Task<T?> Parse<T>(string filePath) where T : class
	{
		try
		{
			await using var stream = File.OpenRead(filePath);
			var serializer = new XmlSerializer(typeof(T));
			return (T?)serializer.Deserialize(stream);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to read or deserialize XML file: {FilePath}", filePath);
			return default;
		}
	}

}
