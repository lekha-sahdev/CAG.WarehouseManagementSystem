using CAG.WarehouseManagementSystem.Dtos;

namespace CAG.WarehouseManagementSystem.Services
{
	public interface ICustomerService : ITransient
	{
		Task<CustomerDto> CreateCustomerAsync(CustomerDto customer);
		Task<bool> DeleteCustomerAsync(int id);
		Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
		Task<CustomerDto> GetCustomerByIdAsync(int id);
		Task<bool> UpdateCustomerAsync(int id, UpdateCustomerDto customer);
	}
}