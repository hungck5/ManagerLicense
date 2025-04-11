using Microsoft.AspNetCore.Mvc;

public class LicenseController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _baseApiUrl;

    public LicenseController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _baseApiUrl = configuration["ApiSettings:BaseUrl"];
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LicenseInfo licenseInfo)
    {
        if (string.IsNullOrWhiteSpace(licenseInfo.LicenseKey))
        {
            ViewBag.Message = "License key is required.";
            return View();
        }

        try 
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseApiUrl}/License/Verify", new { licenseKey = licenseInfo.LicenseKey });
            var content = await response.Content.ReadAsStringAsync();
            ViewBag.Message = content;
        }
        catch (Exception ex)
        {
            ViewBag.Message = "Error verifying license: " + ex.Message;
        }

        return View();
    }
}
