namespace CAG.WarehouseManagementSystem.Data.Repositories
{
	using global::CAG.WarehouseManagementSystem.Data.Entities;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	namespace CAG.WarehouseManagementSystem.Data.Repositories
	{
		public class CustomerRepository : ICustomerRepository
		{
			private readonly WareHouseDbContext _context;

			public CustomerRepository(WareHouseDbContext context)
			{
				_context = context;
			}

			public async Task<IEnumerable<Customer>> GetAllAsync()
			{
				return await _context.Customers.ToListAsync();
			}

			public async Task<Customer?> GetByIdAsync(int id)
			{
				return await _context.Customers.FindAsync(id);
			}

			public async Task<Customer> AddAsync(Customer customer)
			{
				_context.Customers.Add(customer);
				await _context.SaveChangesAsync();
				return customer;
			}

			public async Task<bool> UpdateAsync(Customer customer)
			{
				_context.Entry(customer).State = EntityState.Modified;

				try
				{
					await _context.SaveChangesAsync();
					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!Exists(customer.CustomerId))
					{
						return false;
					}

					throw;
				}
			}

			public async Task<bool> DeleteAsync(int id)
			{
				var customer = await _context.Customers.FindAsync(id);
				if (customer == null)
				{
					return false;
				}

				_context.Customers.Remove(customer);
				await _context.SaveChangesAsync();
				return true;
			}

			private bool Exists(int id)
			{
				return _context.Customers.Any(e => e.CustomerId == id);
			}
		}
	}

}
