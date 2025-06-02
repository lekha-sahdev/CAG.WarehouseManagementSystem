using AutoMapper;
using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.ExceptionManagement;

namespace CAG.WarehouseManagementSystem.Services
{
	public class SalesOrderService(ISalesOrderRepository SalesOrderRepository, IMapper mapper) : ISalesOrderService
	{
		private readonly ISalesOrderRepository _SalesOrderRepository = SalesOrderRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<IEnumerable<SalesOrderDto>> GetAllSalesOrdersAsync()
		{
			var SalesOrders = await _SalesOrderRepository.GetAllAsyncWithOrders();
			return _mapper.Map<IEnumerable<SalesOrderDto>>(SalesOrders);
		}

		public async Task<SalesOrderDto> GetSalesOrderByIdAsync(int id)
		{
			var SalesOrder = await _SalesOrderRepository.GetByIdAsyncWithOrders(id);
			if (SalesOrder == null)
				throw new CagBusinessException(Constants.Not_Found_Msg, Constants.Not_Found_Detailed_Msg, System.Net.HttpStatusCode.NotFound);
			return _mapper.Map<SalesOrderDto>(SalesOrder);
		}

		public async Task<SalesOrderDto> CreateSalesOrderAsync(SalesOrderDto SalesOrderDto)
		{
			var SalesOrder = _mapper.Map<SalesOrder>(SalesOrderDto);
			var created = await _SalesOrderRepository.AddAsync(SalesOrder);
			return _mapper.Map<SalesOrderDto>(created);
		}
	}
}
