using EcommerceApi.Db;
using EcommerceApi.Models;
using EcommerceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace EcommerceApi.Services;

public class ProductCategoryService : IProductCategoryService
{
  private readonly ApplicationDbContext _context;

    public ProductCategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

  public async Task<ProductCategoryDTO?> CreateProductCategoryAsync(ProductCategoryDTO productCategoryDTO)
  {
    var productCategory = new ProductCategory
    {
      Name = productCategoryDTO.Name,
      Description = productCategoryDTO.Description
    };

    await _context.ProductCategories.AddAsync(productCategory);
    await _context.SaveChangesAsync();
    return productCategoryDTO;
  }

  public async Task<bool> DeleteProductCategoryAsync(Guid id)
  {
    var category = await _context.ProductCategories.FindAsync(id);
    if (category == null) return false;

    _context.ProductCategories.Remove(category);
    await _context.SaveChangesAsync();
    return true;
  }

  public async Task<IEnumerable<ProductCategoryDTO>> GetAllProductCategoriesAsync()
  {
    return await _context.ProductCategories
      .Select(c => new ProductCategoryDTO
      {
        Id = c.Id,
        Name = c.Name,
        Description = c.Description,
        ImageUrl = c.Image,
        SeoMetaDTO = new SeoMetaDTO
        {
          MetaTitle = c.SeoMeta.MetaTitle,
          MetaDescription = c.SeoMeta.MetaDescription,
          Keywords = c.SeoMeta.Keywords,
          Author = c.SeoMeta.Author,
          Robots = c.SeoMeta.Robots,
          CanonicalUrl = c.SeoMeta.CanonicalUrl,
          OgTitle = c.SeoMeta.OgTitle,
          OgDescription = c.SeoMeta.OgDescription,
          OgImage = c.SeoMeta.OgImage,
          OgUrl = c.SeoMeta.OgUrl,
          Slug = c.SeoMeta.Slug
        }
      })
      .ToListAsync();
  }

  public async Task<ProductCategoryDTO?> GetProductCategoryByIdAsync(Guid id)
  {
    var category = await _context.ProductCategories.FindAsync(id);
    if (category == null) return null;

    return new ProductCategoryDTO
    {
      Id = category.Id,
      Name = category.Name,
      Description = category.Description,
      ImageUrl = category.Image,
      SeoMetaDTO = new SeoMetaDTO
      {
        MetaTitle = category.SeoMeta.MetaTitle,
        MetaDescription = category.SeoMeta.MetaDescription,
        Keywords = category.SeoMeta.Keywords,
        Author = category.SeoMeta.Author,
        Robots = category.SeoMeta.Robots,
        CanonicalUrl = category.SeoMeta.CanonicalUrl,
        OgTitle = category.SeoMeta.OgTitle,
        OgDescription = category.SeoMeta.OgDescription,
        OgImage = category.SeoMeta.OgImage,
        OgUrl = category.SeoMeta.OgUrl,
        Slug = category.SeoMeta.Slug
      }
    };
  }

  public async Task<ProductCategoryDTO?> UpdateProductCategoryAsync(ProductCategoryDTO productCategoryDTO)
  {
    var existingCategory = await _context.ProductCategories.FindAsync(productCategoryDTO.Id);
    if (existingCategory == null) return null;

    existingCategory.Update(
        productCategoryDTO.Name,
        productCategoryDTO.Description,
        productCategoryDTO.ImageUrl,
        productCategoryDTO.SeoMetaDTO.MetaTitle,
        productCategoryDTO.SeoMetaDTO.MetaDescription,
        productCategoryDTO.SeoMetaDTO.Keywords,
        productCategoryDTO.SeoMetaDTO.Author,
        productCategoryDTO.SeoMetaDTO.Robots,
        productCategoryDTO.SeoMetaDTO.CanonicalUrl,
        productCategoryDTO.SeoMetaDTO.OgTitle,
        productCategoryDTO.SeoMetaDTO.OgDescription,
        productCategoryDTO.SeoMetaDTO.OgImage,
        productCategoryDTO.SeoMetaDTO.OgUrl,
        productCategoryDTO.SeoMetaDTO.Slug
    );

    await _context.SaveChangesAsync();
    return new ProductCategoryDTO
    {
      Id = existingCategory.Id,
      Name = existingCategory.Name,
      Description = existingCategory.Description,
      ImageUrl = existingCategory.Image,
      SeoMetaDTO = new SeoMetaDTO
      {
          MetaTitle = existingCategory.SeoMeta.MetaTitle,
          MetaDescription = existingCategory.SeoMeta.MetaDescription,
          Keywords = existingCategory.SeoMeta.Keywords,
          Author = existingCategory.SeoMeta.Author,
          Robots = existingCategory.SeoMeta.Robots,
          CanonicalUrl = existingCategory.SeoMeta.CanonicalUrl,
          OgTitle = existingCategory.SeoMeta.OgTitle,
          OgDescription = existingCategory.SeoMeta.OgDescription,
          OgImage = existingCategory.SeoMeta.OgImage,
          OgUrl = existingCategory.SeoMeta.OgUrl,
          Slug = existingCategory.SeoMeta.Slug
      }
    };
  }
}