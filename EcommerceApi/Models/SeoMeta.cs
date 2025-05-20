using EcommerceApi.Framework.Domain;

namespace EcommerceApi.Models;

public class SeoMeta : AggregateRoot
{
    public string MetaTitle { get; set; }
    public string MetaDescription { get; set; }
    public string Keywords { get; set; }
    public string Author { get; set; }
    public string Robots { get; set; }
    public string CanonicalUrl { get; set; }
    public string OgTitle { get; set; }
    public string OgDescription { get; set; }
    public string OgImage { get; set; }
    public string OgUrl { get; set; }
    public string Slug { get; set; }
    public string? Title { get; set; }
    public string? ContentHtml { get; set; }
    public MenuItem? MenuItem { get; set; }
    public ProductCategory? ProductCategory { get; set; }
    public Product? Product { get; set; }
}
