using Autofac;
using Autofac.Extensions.DependencyInjection;
using CAG.WarehouseManagementSystem.ExceptionManagement;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[ExcludeFromCodeCoverage]
internal class Program
{
	private const string ASSEMBLY_NAME_PATTERN = "CAG.";
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

		// Add services to the container.
		builder.Services.AddControllers(opt => opt.Filters.Add<CagExceptionFilter>());

		builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
		{
			IEnumerable<Assembly> assemblies = GetAssemblies();
			foreach (var type in GetAssemblyTypes<ITransient>(assemblies, false))
			{
				containerBuilder.RegisterType(type).AsImplementedInterfaces().InstancePerDependency();
			}
			foreach (var type in GetAssemblyTypes<ITransient>(assemblies, true))
			{
				containerBuilder.RegisterGeneric(type).AsImplementedInterfaces().InstancePerDependency();
			}
			foreach (var type in GetAssemblyTypes<IScoped>(assemblies, false))
			{
				containerBuilder.RegisterType(type).AsImplementedInterfaces().InstancePerLifetimeScope();
			}
			foreach (var type in GetAssemblyTypes<IScoped>(assemblies, true))
			{
				containerBuilder.RegisterGeneric(type).As(type.GetInterfaces().First()).InstancePerLifetimeScope();
			}

		});

		SetDataBase(builder);

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		var app = builder.Build();
		SetupDatabase(app);

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

	private static void SetupDatabase(WebApplication app)
	{
		using (var scope = app.Services.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<WareHouseDbContext>();
			db.Database.EnsureCreated();
		}
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

	private static void SetDataBase(WebApplicationBuilder builder) {

		SqliteConnection? sharedSqliteConnection = null;
		if (builder.Configuration.GetValue<string>("DatabaseType") == "SqlLite")
		{
			sharedSqliteConnection = new SqliteConnection("DataSource=:memory:");
			sharedSqliteConnection.Open(); 
		}
		builder.Services.AddDbContext<WareHouseDbContext>(options =>
		{
			if (sharedSqliteConnection != null)
				options.UseSqlite(sharedSqliteConnection);


			else if (builder.Configuration.GetValue<string>("DatabaseType")!.Equals("InMemory"))
				options.UseInMemoryDatabase("WareHouseDb");
			else
				options.UseSqlServer(builder.Configuration.GetConnectionString("WarehouseDbConnection"));	
		});	
	}
}