public interface IFileParser
{
	Task<T?> Parse<T>(string filePath) where T : class;
}
