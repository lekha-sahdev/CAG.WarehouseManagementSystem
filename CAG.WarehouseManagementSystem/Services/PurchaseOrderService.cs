using AutoMapper;
using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.ExceptionManagement;

namespace CAG.WarehouseManagementSystem.Services
{
	public class PurchaseOrderService(IPurchaseOrderRepository PurchaseOrderRepository, IMapper mapper) : IPurchaseOrderService
	{
		private readonly IPurchaseOrderRepository _PurchaseOrderRepository = PurchaseOrderRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrdersAsync()
		{
			var PurchaseOrders = await _PurchaseOrderRepository.GetAllAsyncWithOrders();
			return _mapper.Map<IEnumerable<PurchaseOrderDto>>(PurchaseOrders);
		}

		public async Task<PurchaseOrderDto> GetPurchaseOrderByIdAsync(int id)
		{
			var PurchaseOrder = await _PurchaseOrderRepository.GetByIdAsyncWithOrders(id);
			if (PurchaseOrder == null)
				throw new CagBusinessException(Constants.Not_Found_Msg, Constants.Not_Found_Detailed_Msg, System.Net.HttpStatusCode.NotFound);
			return _mapper.Map<PurchaseOrderDto>(PurchaseOrder);
		}

		public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(PurchaseOrderDto PurchaseOrderDto)
		{
			var PurchaseOrder = _mapper.Map<PurchaseOrder>(PurchaseOrderDto);
			var created = await _PurchaseOrderRepository.AddAsync(PurchaseOrder);
			return _mapper.Map<PurchaseOrderDto>(created);
		}
	}
}
