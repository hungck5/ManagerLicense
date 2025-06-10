using EcommerceApi.Models;
namespace EcommerceApi.Services.Interfaces;

public interface IProductCategoryService
{
  Task<ProductCategoryDTO?> GetProductCategoryByIdAsync(Guid id);
  Task<IEnumerable<ProductCategoryDTO>> GetAllProductCategoriesAsync();
  Task<ProductCategoryDTO?> CreateProductCategoryAsync(ProductCategoryDTO productCategoryDTO);
  Task<ProductCategoryDTO?> UpdateProductCategoryAsync(ProductCategoryDTO productCategoryDTO);
  Task<bool> DeleteProductCategoryAsync(Guid id);
}
