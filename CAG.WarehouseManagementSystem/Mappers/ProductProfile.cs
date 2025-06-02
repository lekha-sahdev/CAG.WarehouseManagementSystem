using AutoMapper;
using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;

namespace CAG.WarehouseManagementSystem.MappingProfiles
{
	public class ProductProfile : Profile
	{
		public ProductProfile()
		{
			CreateMap<Product, ProductDto>().ReverseMap();
			CreateMap<Product, UpdateProductDto>().ReverseMap();
		}
	}
}
