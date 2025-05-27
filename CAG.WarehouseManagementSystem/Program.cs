using Autofac;
using Autofac.Extensions.DependencyInjection;
using CAG.WarehouseManagementSystem.ExceptionManagement;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


internal class Program
{
	private const string ASSEMBLY_NAME_PATTERN = "CAG.";
	private static void Main(string[] args)
	{
		//TODO Lekha: Pending JWT, Validator
		var builder = WebApplication.CreateBuilder(args);

		// Replace default service provider with Autofac
		builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

		// Add services to the container.
		builder.Services.AddControllers(opt => opt.Filters.Add<CagExceptionFilter>());

		builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
		{
			foreach (var type in GetAssemblyTypes<ITransient>())
			{
				containerBuilder.RegisterType(type)
				.AsImplementedInterfaces()
				.InstancePerDependency();
			}
		});

		builder.Services.AddDbContext<WareHouseDbContext>(a => updateDatabaseConfiguration(builder, a));

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();

	}

	private static void updateDatabaseConfiguration(WebApplicationBuilder builder, DbContextOptionsBuilder a)
	{
		if (builder.Configuration.GetValue<string>("DatabaseType")!.Equals("InMemory"))
			a.UseInMemoryDatabase(databaseName: "WareHouseDb");
		else
			a.UseSqlServer(builder.Configuration.GetConnectionString("WarehouseDbConnection"));
	}

	private static IEnumerable<Assembly> GetAssemblies()
	{
		var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
		var assemblies = allAssemblies
			.Where(x => x.GetName().Name!.StartsWith(ASSEMBLY_NAME_PATTERN, StringComparison.InvariantCultureIgnoreCase));
		return assemblies;
	}

	private static IEnumerable<Type> GetAssemblyTypes<TDependecyType>()
	{
		IEnumerable<Assembly> enumerable = GetAssemblies();
		return enumerable
			.SelectMany(x => x.DefinedTypes)
			.Where(x => x.IsClass)
			.Where(t => typeof(TDependecyType).IsAssignableFrom(t));
	}
}