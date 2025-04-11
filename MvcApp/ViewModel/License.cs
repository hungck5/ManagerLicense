using System.Text.Json.Serialization;
public class License
{
    [JsonPropertyName("licenseKey")]
    public string LicenseKey { get; set; }
    [JsonPropertyName("expirationDate")]
    public DateTime ExpirationDate { get; set; }
    [JsonPropertyName("user")]
    public string User { get; set; }
    [JsonPropertyName("signature")]
    public string Signature { get; set; }
}