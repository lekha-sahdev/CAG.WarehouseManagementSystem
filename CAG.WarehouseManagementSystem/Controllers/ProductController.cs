using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAG.WarehouseManagementSystem.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ProductController(IProductService productService, ILogger<ProductController> logger) : ControllerBase
	{
		private readonly IProductService _productService = productService;
		private readonly ILogger<ProductController> _logger = logger;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
		{
			var products = await _productService.GetAllProductsAsync();
			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDto>> GetProduct(int id)
		{
			var product = await _productService.GetProductByIdAsync(id);
			_logger.LogInformation($"Product with ID: {id} found - {product == null}");	
			return product  == null ? NoContent() : Ok(product);
		}

		[HttpPost]
		public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
		{
			var createdProduct = await _productService.CreateProductAsync(productDto);
			_logger.LogInformation($"Product created with ID: {createdProduct.Id}");
			return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct(int id, UpdateProductDto productDto)
		{
			var success = await _productService.UpdateProductAsync(id, productDto);
			_logger.LogInformation($"Product with ID: {id} updated - {success}");
			return success ? NoContent() : NotFound();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var success = await _productService.DeleteProductAsync(id);
			_logger.LogInformation($"Product with ID: {id} deleted - {success}");	
			return success ? NoContent() : NotFound();
		}
	}
}
