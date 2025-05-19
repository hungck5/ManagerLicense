namespace EcommerceApi.Models;

public class ProductCategory
{
    public ProductCategory() { }

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

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Image { get; private set; }
    public SeoMeta SeoMeta { get; private set; }
}
