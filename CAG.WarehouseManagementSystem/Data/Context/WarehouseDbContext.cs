using CAG.WarehouseManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class WareHouseDbContext : DbContext
{
	public WareHouseDbContext(DbContextOptions<WareHouseDbContext> options)
		: base(options) { }

	public DbSet<Customer> Customers { get; set; }

	public DbSet<Product> Products { get; set; }

	public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
	public DbSet<PurchaseOrderProduct> PurchaseOrderProducts { get; set; }

	public DbSet<SalesOrder> SalesOrders { get; set; }
	public DbSet<SalesOrderProduct> SalesOrderProducts { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PurchaseOrderProduct>()
		.HasKey(oi => new { oi.PurchaseOrderId, oi.Id });

		modelBuilder.Entity<SalesOrderProduct>()
			.HasKey(oi => new { oi.SalesOrderId, oi.Id });
	}
}