using System.Net.Http.Json;
using MvcApp.Models;

namespace MvcApp.Services;

public class ArticleApiClient(HttpClient httpClient, IConfiguration configuration)
{
    private readonly string _baseUrl = (configuration["ArticleApi:BaseUrl"] ?? "https://localhost:7130/api").TrimEnd('/');

    public async Task<IReadOnlyList<Article>> GetPublishedAsync(CancellationToken cancellationToken = default) =>
        await httpClient.GetFromJsonAsync<List<Article>>($"{_baseUrl}/articles?publishedOnly=true", cancellationToken) ?? [];

    public async Task<Article?> GetPublishedBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"{_baseUrl}/articles/{Uri.EscapeDataString(slug)}?publishedOnly=true", cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Article>(cancellationToken);
    }
}
