using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data;
using EcommerceApi.Models;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.DTOs;

namespace EcommerceApi.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(ProductDTO product)
    {
        var newProduct = new Product(Guid.NewGuid(),
                                     product.Name,
                                     product.Unit,
                                     product.VAT,
                                     product.Discount,
                                     product.Status,
                                     product.Origin,
                                     product.Preservation,
                                     product.Supplier,
                                     product.Notes,
                                     product.Description,
                                     product.Price,
                                     product.Image,
                                     product.Category);
        
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        return newProduct;
    }

    public async Task<bool> UpdateAsync(Guid id, ProductDTO product)
    {
        if (id != product.Id) return false;

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Products.AnyAsync(p => p.Id == id))
                return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
