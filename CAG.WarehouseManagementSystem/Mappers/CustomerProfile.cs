namespace CAG.WarehouseManagementSystem.Mappers
{
	using AutoMapper;
	using CAG.WarehouseManagementSystem.Data.Entities;
	using CAG.WarehouseManagementSystem.Dtos;

	public class CustomerProfile : Profile
	{
		public CustomerProfile()
		{

			CreateMap<Customer, CustomerDto>().ReverseMap();
			CreateMap<Customer, UpdateCustomerDto>().ReverseMap();
		}
	}
}
