using MvcApp.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace MvcApp.Services;

public class LicenseVerifierService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IWebHostEnvironment _env;
    private static LicenseStatus _status = new LicenseStatus();

    public static LicenseStatus Status => _status;
    private readonly LicenseSettings _settings;
    private readonly ILogger<LicenseVerifierService> _logger;

    public LicenseVerifierService(IHttpClientFactory httpClientFactory, IWebHostEnvironment env,
                                  IOptions<LicenseSettings> options, ILogger<LicenseVerifierService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _env = env;
        _settings = options.Value;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
      _logger.LogInformation("License verification started at: {time}", DateTimeOffset.Now);
      var licensePath = Path.Combine(_env.ContentRootPath, "license.json");
      string licenseContent = null;

      if (!File.Exists(licensePath))
      {
          // 🆕 Gọi App A để lấy license mới
          var newLicense = await FetchNewLicense(_settings.UsernameDefault); // user nào đó
          if (newLicense != null)
          {
              await File.WriteAllTextAsync(licensePath, newLicense);
              licenseContent = newLicense;
          }
          else
          {
              _status = new LicenseStatus { VerifyResult = "❌ Không lấy được license từ App A" };
              return;
          }
      }
      else
      {
          licenseContent = await File.ReadAllTextAsync(licensePath);
      }

      try
      {
          var http = _httpClientFactory.CreateClient();
          var response = await http.PostAsJsonAsync($"{_settings.BaseUrl}/License/Verify", new { LicenseKey = licenseContent });

          var result = await response.Content.ReadAsStringAsync();
          _status = new LicenseStatus
          {
              LicenseContent = licenseContent,
              VerifyResult = response.IsSuccessStatusCode ? "✅ License hợp lệ" : $"❌ Không hợp lệ: {result}",
              LastChecked = DateTime.Now
          };
          _logger.LogInformation("License verification result: {result}", _status.VerifyResult);
      }
      catch (Exception ex)
      {
          _logger.LogError(ex, "Error verifying license: {message}", ex.Message);
          _status = new LicenseStatus
          {
              LicenseContent = licenseContent,
              VerifyResult = "❌ Lỗi khi gọi Verify: " + ex.Message,
              LastChecked = DateTime.Now
          };
      }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private async Task<string> FetchNewLicense(string username)
    {
      _logger.LogInformation("Fetching new license for user: {username}", username);
        try
        {
          var http = _httpClientFactory.CreateClient();
            var response = await http.GetAsync($"{_settings.BaseUrl}/License/GetLicenseInfor?user={username}");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("New license fetched successfully: {json}", json);
            return json;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching new license: {message}", ex.Message);
            return null;
        }
    }
}
