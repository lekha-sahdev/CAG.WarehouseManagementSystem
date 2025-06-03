namespace CAG.LegacyWareHouseMngntSystem.Jobs
{
	using Microsoft.Extensions.Logging;
	using Quartz;
	using Renci.SshNet;
	using System.Threading.Tasks;

	[DisallowConcurrentExecution]
	public class FilePollingJob : IJob
	{
		private readonly ILogger<FilePollingJob> _logger;
		private readonly IConfiguration _configuration;
		private readonly IFileProcessService _fileProcessService;
		public FilePollingJob(ILogger<FilePollingJob> logger, IConfiguration configuration, IFileProcessService fileProcessService)
		{
			_logger = logger;
			_configuration = configuration;
			_fileProcessService = fileProcessService;
		}

		public async Task Execute(IJobExecutionContext context)
		{
				var folder = _configuration.GetValue<string>("Polling:LocalPath");
				var files = Directory.GetFiles(folder);
				foreach (var file in files)
				{
					Console.WriteLine($"[Local] Found file: {Path.GetFileName(file)}");
					await _fileProcessService.ProcessFileAsync(file);
				}
			
		}
	}
}
