
public interface IFileProcessService : ISingleton
{
	Task ProcessFileAsync(string filePath);
}