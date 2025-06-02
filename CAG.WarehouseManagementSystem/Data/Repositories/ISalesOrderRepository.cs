using CAG.WarehouseManagementSystem.Data.Entities;

public interface ISalesOrderRepository : IRepository<SalesOrder>
{
	Task<IEnumerable<SalesOrder>> GetAllAsyncWithOrders();
	Task<SalesOrder?> GetByIdAsyncWithOrders(int id);
}