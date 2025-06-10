

using EcommerceApi.Models;

namespace EcommerceApi.Services;

public class ProductCategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public Guid ParentId { get; set; }
    public SeoMetaDTO SeoMetaDTO { get; set; }
}