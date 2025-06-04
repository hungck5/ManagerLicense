using EcommerceApi.Models;
namespace EcommerceApi.Services.Interfaces;

public interface IProductCategoryService
{
  Task<ProductCategory?> GetProductCategoryByIdAsync(Guid id);
  Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync();
  Task<ProductCategory?> CreateProductCategoryAsync(ProductCategory productCategory);
  Task<ProductCategory?> UpdateProductCategoryAsync(ProductCategory productCategory);
  Task<bool> DeleteProductCategoryAsync(Guid id);
}
