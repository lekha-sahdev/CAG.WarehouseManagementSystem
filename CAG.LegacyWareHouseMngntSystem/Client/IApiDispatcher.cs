public interface IApiDispatcher : ISingleton
{
	Task DispatchAsync<T>(T dto, string url);
}
