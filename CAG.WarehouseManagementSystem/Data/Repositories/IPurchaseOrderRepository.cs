using CAG.WarehouseManagementSystem.Data.Entities;

public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
{
	Task<IEnumerable<PurchaseOrder>> GetAllAsyncWithOrders();
	Task<PurchaseOrder?> GetByIdAsyncWithOrders(int id);
}