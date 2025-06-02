using CAG.WarehouseManagementSystem.Dtos;

namespace CAG.WarehouseManagementSystem.Services
{
	public interface ISalesOrderService : ITransient
	{
		Task<SalesOrderDto> CreateSalesOrderAsync(SalesOrderDto SalesOrderDto);
		Task<IEnumerable<SalesOrderDto>> GetAllSalesOrdersAsync();
		Task<SalesOrderDto> GetSalesOrderByIdAsync(int id);
	}
}