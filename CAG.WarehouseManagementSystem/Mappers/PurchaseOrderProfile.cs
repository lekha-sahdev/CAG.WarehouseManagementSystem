using AutoMapper;
using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;

namespace CAG.WarehouseManagementSystem.MappingProfiles
{
	public class PurchaseOrderProfile : Profile
	{
		public PurchaseOrderProfile()
		{
			CreateMap<PurchaseOrder, PurchaseOrderDto>()
				.ForMember(t => t.OrdersDto, m => m.MapFrom(s => s.PurchaseOrderProducts))
				.ReverseMap()
				.ForMember(p => p.PurchaseOrderProducts, po => po.MapFrom(src => src.OrdersDto));

			CreateMap<PurchaseOrderProduct, OrderDto>().ReverseMap();
		}
	}
}
