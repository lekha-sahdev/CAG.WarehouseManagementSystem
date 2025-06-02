using AutoMapper;
using CAG.WarehouseManagementSystem.Data.Entities;
using CAG.WarehouseManagementSystem.Dtos;
using CAG.WarehouseManagementSystem.ExceptionManagement;

namespace CAG.WarehouseManagementSystem.Services
{
	public class ProductService(IRepository<Product> productRepository, IMapper mapper) : IProductService
	{
		private readonly IRepository<Product> _productRepository = productRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
		{
			var Products = await _productRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ProductDto>>(Products);
		}

		public async Task<ProductDto> GetProductByIdAsync(int id)
		{
			var Product = await _productRepository.GetByIdAsync(id);
			if (Product == null)
				throw new CagBusinessException(Constants.Not_Found_Msg, Constants.Not_Found_Detailed_Msg, System.Net.HttpStatusCode.NotFound);
			return _mapper.Map<ProductDto>(Product);
		}

		public async Task<ProductDto> CreateProductAsync(ProductDto ProductDto)
		{
			var Product = _mapper.Map<Product>(ProductDto);
			var created = await _productRepository.AddAsync(Product);
			return _mapper.Map<ProductDto>(created);
		}

		public async Task<bool> UpdateProductAsync(int id, UpdateProductDto ProductDto)
		{
			var Product = _mapper.Map<Product>(ProductDto);
			Product.Id = id;
			return await _productRepository.UpdateAsync(Product);
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			return await _productRepository.DeleteAsync(id);
		}
	}
}
