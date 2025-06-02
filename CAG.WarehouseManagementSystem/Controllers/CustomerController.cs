using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAG.WarehouseManagementSystem.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class CustomerController(ICustomerService customerService, ILogger<CustomerController> logger) : ControllerBase
	{
		private readonly ICustomerService _customerService = customerService;
		private readonly ILogger<CustomerController> _logger = logger;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
		{
			var customers = await _customerService.GetAllCustomersAsync();
			return Ok(customers);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
		{
			_logger.LogInformation($"Fetching customer with ID: {id}");
			var customer = await _customerService.GetCustomerByIdAsync(id);
			_logger.LogInformation($"Retrieved customer with ID : {customer.Id}");
			return Ok(customer);
		}

		[HttpPost]
		public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
		{
			var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
			_logger.LogInformation($"Created new customer with ID: {createdCustomer.Id}");
			return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCustomer(int id, UpdateCustomerDto customerDto)
		{
			var success = await _customerService.UpdateCustomerAsync(id, customerDto);
			_logger.LogInformation($"Product with ID: {id} updated - {success}");
			return success ? NoContent() : NotFound();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCustomer(int id)
		{
			var success = await _customerService.DeleteCustomerAsync(id);
			_logger.LogInformation($"Product with ID: {id} deleted - {success}");
			return success ? NoContent() : NotFound();
		}
	}
}