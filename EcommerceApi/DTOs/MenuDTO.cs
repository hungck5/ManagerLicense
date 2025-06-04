namespace EcommerceApi.DTOs;

public class MenuItemDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Icon { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public int ParentId { get; set; }
}