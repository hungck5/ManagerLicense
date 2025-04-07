using LicenseManagerApp.Models;
using Microsoft.AspNetCore.Mvc;

public class LicenseController : Controller
{
    private static List<License> licenses = new List<License>();

    // Get list of licenses
    public IActionResult Index()
    {
        return View(licenses);
    }

    // Create license
    [HttpPost]
    public IActionResult Create(string user)
    {
        var licenseKey = Guid.NewGuid().ToString();
        var license = new License
        {
            LicenseKey = licenseKey,
            ExpirationDate = DateTime.Now.AddYears(1),
            IsActive = true,
            User = user
        };
        licenses.Add(license);
        return RedirectToAction("Index");
    }

    // Get license by key (for verification)
    public IActionResult Verify(string licenseKey)
    {
        var license = licenses.FirstOrDefault(l => l.LicenseKey == licenseKey);
        if (license != null && license.IsActive && license.ExpirationDate > DateTime.Now)
        {
            return Ok("License is valid.");
        }
        return BadRequest("Invalid license.");
    }
    

    public IActionResult GetActiveLicenses()
    {
        var activeLicenses = licenses.Where(l => l.IsActive).ToList();
        return View(activeLicenses);
    }
}
