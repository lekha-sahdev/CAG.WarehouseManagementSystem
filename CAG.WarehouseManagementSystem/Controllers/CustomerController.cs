using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAG.WarehouseManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		private readonly ICustomerService _customerService;
		private readonly ILogger<CustomerController> _logger;

		public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
		{
			_customerService = customerService;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
		{
			var customers = await _customerService.GetAllCustomersAsync();
			return Ok(customers);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
		{
			var customer = await _customerService.GetCustomerByIdAsync(id);
			return Ok(customer);
		}

		[HttpPost]
		public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
		{
			var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
			return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutCustomer(int id, UpdateCustomerDto customerDto)
		{
			var success = await _customerService.UpdateCustomerAsync(id, customerDto);
			if (!success)
			{
				return NotFound();
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCustomer(int id)
		{
			var success = await _customerService.DeleteCustomerAsync(id);
			if (!success)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}