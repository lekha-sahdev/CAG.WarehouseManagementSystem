using Autofac;
using Autofac.Extensions.DependencyInjection;
using CAG.LegacyWareHouseMngntSystem;
using CAG.LegacyWareHouseMngntSystem.Jobs;
using Polly;
using Quartz;
using Quartz.Spi;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;



[ExcludeFromCodeCoverage]
class Program
{
	private const string ASSEMBLY_NAME_PATTERN = "CAG.";
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Use Autofac
		builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

		builder.Host.ConfigureContainer<ContainerBuilder>(container =>
		{
			container.RegisterType<FilePollingJob>().InstancePerDependency();
			container.RegisterType<AutofacJobFactory>().As<IJobFactory>();
		});

		var cron = builder.Configuration.GetValue<string>("Polling:CRON") ?? "0/30 * * * * ?"; // every 30 seconds as default

		builder.Services.AddQuartz(q =>
		{
			q.UseJobFactory<AutofacJobFactory>();

			var jobKey = new JobKey("FilePollingJob");
			q.AddJob<FilePollingJob>(opts => opts.WithIdentity(jobKey));
			q.AddTrigger(opts => opts
				.ForJob(jobKey)
				.WithIdentity("FilePollingJob-trigger")
				.WithCronSchedule(cron));
		});

		// Register Polly policy
		builder.Host.ConfigureContainer<ContainerBuilder>(container =>
		{
			var config = builder.Configuration;
			var retryCount = config.GetValue<int>("Polling:RetryPolicy:RetryCount");
			var retryDelay = config.GetValue<int>("Polling:RetryPolicy:RetryDelaySeconds");
			container.Register(ctx =>
			Policy
			.Handle<Exception>()
			.WaitAndRetryAsync(
				retryCount,
				retryAttempt => TimeSpan.FromSeconds(retryDelay)
			)
			).As<AsyncPolicy>().SingleInstance();
		});

		builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
		builder.Services.AddLogging();
		builder.Services.AddHttpClient();

		builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
		{
			IEnumerable<Assembly> assemblies = GetAssemblies();
			foreach (var type in GetAssemblyTypes<ISingleton>(assemblies, false))
			{
				containerBuilder.RegisterType(type).AsImplementedInterfaces().InstancePerDependency();
			}

			foreach (var type in GetAssemblyTypes<IFileParser>(assemblies, false))
			{
				containerBuilder.RegisterType(type).Keyed<IFileParser>(type.Name.Replace("Parser", "").ToLower()).InstancePerDependency();
			}

		});

		var app = builder.Build();
		app.Run();
	}

	private static IEnumerable<Assembly> GetAssemblies()
	{
		var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
		var assemblies = allAssemblies
			.Where(x => x.GetName().Name!.StartsWith(ASSEMBLY_NAME_PATTERN, StringComparison.InvariantCultureIgnoreCase));
		return assemblies;
	}

	private static IEnumerable<Type> GetAssemblyTypes<TDependecyType>(IEnumerable<Assembly> assemblies, bool isGeneric)
	{
		return assemblies
			.SelectMany(x => x.DefinedTypes)
			.Where(x => x.IsClass)
			.Where(t => typeof(TDependecyType).IsAssignableFrom(t))
			.Where(a => a.IsGenericTypeDefinition == isGeneric);
	}
}