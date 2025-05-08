using EcommerceApi.DTOs;
using EcommerceApi.Models;

namespace EcommerceApi.Services.Interfaces;
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(ProductDTO product);
    Task<bool> UpdateAsync(Guid id, ProductDTO product);
    Task<bool> DeleteAsync(Guid id);
}