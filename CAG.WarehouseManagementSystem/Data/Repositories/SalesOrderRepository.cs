using CAG.WarehouseManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class SalesOrderRepository : Repository<SalesOrder>, ISalesOrderRepository
{
	public SalesOrderRepository(WareHouseDbContext context) : base(context)
	{
	}

	public async Task<IEnumerable<SalesOrder>> GetAllAsyncWithOrders()
	{
		return await _dbSet.Include(po => po.SalesOrderProducts).ToListAsync();
	}

	public async Task<SalesOrder?> GetByIdAsyncWithOrders(int id)
	{
		return await _dbSet.Include(po => po.SalesOrderProducts).FirstOrDefaultAsync(po => po.Id == id);
	}


}
