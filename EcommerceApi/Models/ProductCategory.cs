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
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public SeoMeta? SeoMeta { get; set; }
    public Guid? SeoMetaId { get; set; }

    public ICollection<Product> Products { get; set; }
}
