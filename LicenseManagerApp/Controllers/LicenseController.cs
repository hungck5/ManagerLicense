using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using LicenseManagerApp.Models;

public class LicenseController : Controller
{
    private static readonly string PrivateKeyPath = "private.pem";
    private static readonly string PublicKeyPath = "public.pem";
    private static List<License> licenses = new();

    public LicenseController()
    {
        EnsureKeyFilesExist();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(LicenseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.User))
        {
            ViewBag.Message = "User is required.";
            return View();
        }

        var license = new License
        {
            LicenseKey = Guid.NewGuid().ToString(),
            ExpirationDate = DateTime.UtcNow.AddYears(1),
            User = request.User
        };

        string privateKey = System.IO.File.ReadAllText(PrivateKeyPath);
        license.Signature = SignData(license, privateKey);

        licenses.Add(license);
        ViewBag.Message = "License created successfully!";
        ViewBag.License = license;
        return View();
    }

    public IActionResult GetLicenseInfor(string user)
    {
        if (string.IsNullOrWhiteSpace(user))
        {
            return BadRequest("User name is required.");
        }

        var license = licenses.FirstOrDefault(x => x.User == user);
        if (license == null)
        {
            return NotFound("License not found.");
        }

        return Ok(license);
    }
    
    [HttpGet]
    public IActionResult Verify()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Verify([FromBody] VerifyRequest licenseKeyRequest)
    {
        var license = licenses.FirstOrDefault(x => x.LicenseKey == licenseKeyRequest.LicenseKey);
        if (license == null)
        {
            return Ok(new { message = "License not found" });
        }

        string publicKey = System.IO.File.ReadAllText(PublicKeyPath);
        bool isValid = VerifySignature(license, publicKey, license.Signature);

        if (!isValid)
            return BadRequest(new { message = "Invalid license signature" });
        
        if (license.ExpirationDate < DateTime.UtcNow)
            return BadRequest(new { message = "License expired" });
        
        return Ok(new { message = "License valid" });
    }

    [HttpGet]
    public IActionResult GetPublicKey()
    {
        if (!System.IO.File.Exists(PublicKeyPath))
        {
            return NotFound("Public key not found");
        }

        var publicKey = System.IO.File.ReadAllText(PublicKeyPath);
        return Content(publicKey);
    }

    private void EnsureKeyFilesExist()
    {
        if (!System.IO.File.Exists(PrivateKeyPath) || !System.IO.File.Exists(PublicKeyPath))
        {
            using var rsa = RSA.Create(2048);
            string privatePem = rsa.ExportRSAPrivateKeyPem();
            string publicPem = rsa.ExportRSAPublicKeyPem();
            System.IO.File.WriteAllText(PrivateKeyPath, privatePem);
            System.IO.File.WriteAllText(PublicKeyPath, publicPem);
        }
    }

    private string SignData(License license, string privateKeyPem)
    {
        using var rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyPem);
        string data = JsonConvert.SerializeObject(license with { Signature = null });
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(data));
        byte[] signature = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    private bool VerifySignature(License license, string publicKeyPem, string signatureBase64)
    {
        using var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem);
        string data = JsonConvert.SerializeObject(license with { Signature = null });
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(data));
        byte[] signature = Convert.FromBase64String(signatureBase64);
        return rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}
