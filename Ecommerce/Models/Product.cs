namespace ECommerce.Models;
public class Product
{
  public Product() : this(0, string.Empty, 0, string.Empty, string.Empty) { }

  public Product(int id, decimal price, string category) 
      : this(id, string.Empty, price, category, string.Empty) { }

  public Product(string name, decimal price, string category, string description) 
      : this(0, name, price, category, description) { }

  public Product(string name, decimal price, string category) 
      : this(0, name, price, category, string.Empty) { }

  public Product(int id, string name, decimal price, string category, string description) 
  {
    this.Id = id;
    this.Name = name;
    this.Price = price;
    this.Category = category;
    this.Description = description;
  }

  public int Id { get; set; }
  public string Name { get; set; }
  public decimal Price { get; set; }
  public string Description { get; set; }
  public string Category { get; set; }
  
}