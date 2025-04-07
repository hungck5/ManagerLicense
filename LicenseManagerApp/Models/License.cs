namespace LicenseManagerApp.Models;

public class License
{
    public int Id { get; set; }
    public string LicenseKey { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public string User { get; set; }
}