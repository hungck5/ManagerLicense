namespace EcommerceApi.Models;

public class Product
{
  public Product() { }

  public Product(Guid id,
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
                 string image,
                 string category)
  {
    Id = id;
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
    Category = category;
  }

  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public string Unit { get; private set; }
  public decimal VAT { get; private set; }
  public string Status { get; private set; }
  public string Origin { get; private set; }
  public string Preservation { get; private set; }
  public string Supplier { get; private set; }
  public string Notes { get; private set; }
  public string Description { get; private set; }
  public decimal Discount { get; private set; }
  public decimal Price { get; private set; }
  public string Image { get; private set; }
  public string Category { get; private set; }
  
  public SeoMeta SeoMeta { get; set; }
}
