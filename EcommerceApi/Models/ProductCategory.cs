using EcommerceApi.Framework.Domain;

namespace EcommerceApi.Models;

public class ProductCategory : AggregateRoot
{
    public ProductCategory()
    {
        Products = new HashSet<Product>();
        SeoMeta = new SeoMeta();
    }

    public ProductCategory(Guid id,
                            string name,
                            string description,
                            string image)
    {
        Id = id;
        Name = name;
        Description = description;
        Image = image;
        Products = new HashSet<Product>();
        SeoMeta = new SeoMeta();
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public SeoMeta? SeoMeta { get; set; }
    public Guid? SeoMetaId { get; set; }

    public ICollection<Product> Products { get; set; }

    public void Update(string name, 
                        string description, 
                        string image,
                        string seoMetaTitle = null,
                        string seoMetaDescription = null,
                        string seoMetaKeywords = null,
                        string seoMetaAuthor = null,
                        string seoMetaRobots = null,
                        string seoMetaCanonicalUrl = null,
                        string seoMetaOgTitle = null,
                        string seoMetaOgDescription = null,
                        string seoMetaOgImage = null,
                        string seoMetaOgUrl = null,
                        string seoMetaSlug = null)
    {
        Name = name;
        Description = description;
        Image = image;
        SeoMeta = new SeoMeta
        {
            Title = seoMetaTitle,
            MetaDescription = seoMetaDescription,
            Keywords = seoMetaKeywords,
            Author = seoMetaAuthor,
            Robots = seoMetaRobots,
            CanonicalUrl = seoMetaCanonicalUrl,
            OgTitle = seoMetaOgTitle,
            OgDescription = seoMetaOgDescription,
            OgImage = seoMetaOgImage,
            OgUrl = seoMetaOgUrl,
            Slug = seoMetaSlug
        };
    }
}
