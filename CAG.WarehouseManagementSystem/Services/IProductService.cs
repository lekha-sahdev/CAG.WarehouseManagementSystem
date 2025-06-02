using CAG.WarehouseManagementSystem.Dtos;

namespace CAG.WarehouseManagementSystem.Services
{
	public interface IProductService : ITransient
	{
		Task<ProductDto> CreateProductAsync(ProductDto ProductDto);
		Task<bool> DeleteProductAsync(int id);
		Task<IEnumerable<ProductDto>> GetAllProductsAsync();
		Task<ProductDto> GetProductByIdAsync(int id);
		Task<bool> UpdateProductAsync(int id, UpdateProductDto ProductDto);
	}
}