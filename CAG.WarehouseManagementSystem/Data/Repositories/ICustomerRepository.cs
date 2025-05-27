using CAG.WarehouseManagementSystem.Data.Entities;

namespace CAG.WarehouseManagementSystem.Data.Repositories.CAG.WarehouseManagementSystem.Data.Repositories
{
	public interface ICustomerRepository : ITransient
	{
		Task<Customer> AddAsync(Customer customer);
		Task<bool> DeleteAsync(int id);
		Task<IEnumerable<Customer>> GetAllAsync();
		Task<Customer?> GetByIdAsync(int id);
		Task<bool> UpdateAsync(Customer customer);
	}
}