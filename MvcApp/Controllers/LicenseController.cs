using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

public class LicenseController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LicenseController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> VerifyLicense(string licenseKey)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5237/license/verify?licenseKey={licenseKey}");

        if (response.IsSuccessStatusCode)
        {
            ViewBag.Message = "License is valid.";
            return View();
        }

        ViewBag.Message = "Invalid license.";
        return View();
    }
}
