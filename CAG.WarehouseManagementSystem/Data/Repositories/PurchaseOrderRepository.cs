using CAG.WarehouseManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
{
	public PurchaseOrderRepository(WareHouseDbContext context) : base(context)
	{
	}

	public async Task<IEnumerable<PurchaseOrder>> GetAllAsyncWithOrders()
	{
		return await _dbSet.Include(po => po.PurchaseOrderProducts).ToListAsync();
	}

	public async Task<PurchaseOrder?> GetByIdAsyncWithOrders(int id)
	{
		return await _dbSet.Include(po => po.PurchaseOrderProducts).FirstOrDefaultAsync(po => po.Id == id);
	}


}
