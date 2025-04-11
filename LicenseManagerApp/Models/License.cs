namespace LicenseManagerApp.Models;

public record License
{
    public string LicenseKey { get; init; }
    public DateTime ExpirationDate { get; init; }
    public string User { get; init; }
    public string? Signature { get; set; }
}