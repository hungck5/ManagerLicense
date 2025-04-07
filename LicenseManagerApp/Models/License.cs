namespace LicenseManagerApp.Models;

public class License
{
    public int Id { get; set; }
    public string LicenseKey { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
}