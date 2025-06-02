using AutoMapper;
using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;

namespace CAG.WarehouseManagementSystem.MappingProfiles
{
	public class SalesOrderProfile : Profile
	{
		public SalesOrderProfile()
		{
			CreateMap<SalesOrder, SalesOrderDto>()
				.ForMember(t => t.OrdersDto, m => m.MapFrom(s => s.SalesOrderProducts))
				.ReverseMap()
				.ForMember(p => p.SalesOrderProducts, po => po.MapFrom(src => src.OrdersDto));
			CreateMap<SalesOrderProduct, OrderDto>().ReverseMap();
		}
	}
}
