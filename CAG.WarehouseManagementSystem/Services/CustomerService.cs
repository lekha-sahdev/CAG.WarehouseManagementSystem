using AutoMapper;
using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.ExceptionManagement;

namespace CAG.WarehouseManagementSystem.Services
{
	public class CustomerService(IRepository<Customer> customerRepository, IMapper mapper) : ICustomerService
	{
		private readonly IRepository<Customer> _customerRepository = customerRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
		{
			var customers = await _customerRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CustomerDto>>(customers);
		}

		public async Task<CustomerDto> GetCustomerByIdAsync(int id)
		{
			var customer = await _customerRepository.GetByIdAsync(id);
			if (customer == null)
				throw new CagBusinessException(Constants.Not_Found_Msg, Constants.Not_Found_Detailed_Msg, System.Net.HttpStatusCode.NotFound);
			return _mapper.Map<CustomerDto>(customer);
		}

		public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
		{
			var customer = _mapper.Map<Customer>(customerDto);
			var created = await _customerRepository.AddAsync(customer);
			return _mapper.Map<CustomerDto>(created);
		}

		public async Task<bool> UpdateCustomerAsync(int id, UpdateCustomerDto customerDto)
		{
			var customer = _mapper.Map<Customer>(customerDto);
			customer.Id = id;
			return await _customerRepository.UpdateAsync(customer);
		}

		public async Task<bool> DeleteCustomerAsync(int id)
		{
			return await _customerRepository.DeleteAsync(id);
		}
	}
}

