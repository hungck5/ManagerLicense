using EcommerceApi.Framework.Domain;

namespace EcommerceApi.Models;

public class Product : AggregateRoot
{
  public Product() { }

  public Product(
                 string name,
                 string unit,
                 decimal vat,
                decimal discount,
                 string status,
                 string origin,
                 string preservation,
                 string supplier,
                 string notes,
                 string description,
                 decimal price,
                 string image)
  {
    Name = name;
    Unit = unit;
    VAT = vat;
    Status = status;
    Origin = origin;
    Preservation = preservation;
    Supplier = supplier;
    Notes = notes;
    Description = description;
    Discount = discount;
    Price = price;
    Image = image;
  }

  public string Name { get; set; }
  public string Unit { get; set; }
  public decimal VAT { get; set; }
  public string Status { get; set; }
  public string Origin { get; set; }
  public string Preservation { get; set; }
  public string Supplier { get; set; }
  public string Notes { get; set; }
  public string Description { get; set; }
  public decimal Discount { get; set; }
  public decimal Price { get; set; }
  public string Image { get; set; }

  public SeoMeta? SeoMeta { get; set; }
  public Guid? SeoMetaId { get; set; }
  public ProductCategory ProductCategory { get; set; } 
  public Guid ProductCategoryId { get; set; }
}
