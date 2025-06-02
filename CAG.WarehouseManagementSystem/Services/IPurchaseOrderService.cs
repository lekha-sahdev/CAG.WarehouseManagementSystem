using CAG.WarehouseManagementSystem.Dtos;

namespace CAG.WarehouseManagementSystem.Services
{
	public interface IPurchaseOrderService : ITransient
	{
		Task<PurchaseOrderDto> CreatePurchaseOrderAsync(PurchaseOrderDto PurchaseOrderDto);
		Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrdersAsync();
		Task<PurchaseOrderDto> GetPurchaseOrderByIdAsync(int id);
	}
}