namespace EcommerceApi.DTOs;

public class ProductDTO
{
    public Guid Id { get; set; }
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
    public string Category { get; set; }
}
