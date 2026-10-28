using System.Net;
using System.Net.Http.Json;
using ArticleService.Data;
using ArticleService.DTOs;
using ArticleService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace ArticleService.IntegrationTests;

public class ArticlesApiIntegrationTests : IClassFixture<ArticlesApiFactory>
{
    private readonly HttpClient _client;

    public ArticlesApiIntegrationTests(ArticlesApiFactory factory) => _client = factory.CreateClient();

    [Fact]
    public async Task ArticleLifecycle_CreatesFiltersUpdatesAndDeletesThroughHttpApi()
    {
        var draft = await CreateArticleAsync("Draft article", ArticleStatus.Draft);
        var published = await CreateArticleAsync("Published article", ArticleStatus.Published);

        var publishedArticles = await _client.GetFromJsonAsync<List<ArticleDto>>("/api/articles?publishedOnly=true");
        var article = Assert.Single(publishedArticles!);
        Assert.Equal(published.Id, article.Id);
        Assert.Equal("published-article", article.Slug);

        var updatedResponse = await _client.PutAsJsonAsync($"/api/articles/{published.Id}", new SaveArticleRequest
        {
            Title = "Updated article",
            Content = "Updated content",
            Status = ArticleStatus.Draft
        });
        updatedResponse.EnsureSuccessStatusCode();

        var updated = await updatedResponse.Content.ReadFromJsonAsync<ArticleDto>();
        Assert.Equal("updated-article", updated!.Slug);
        Assert.Null(updated.PublishedAt);

        var deleteResponse = await _client.DeleteAsync($"/api/articles/{draft.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _client.GetAsync($"/api/articles/{draft.Slug}")).StatusCode);
    }

    [Fact]
    public async Task UploadImage_RejectsUnsupportedContentType()
    {
        using var form = new MultipartFormDataContent();
        using var file = new ByteArrayContent([1, 2, 3]);
        file.Headers.ContentType = new("text/plain");
        form.Add(file, "file", "not-an-image.txt");

        var response = await _client.PostAsync("/api/articles/images", form);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<ArticleDto> CreateArticleAsync(string title, ArticleStatus status)
    {
        var response = await _client.PostAsJsonAsync("/api/articles", new SaveArticleRequest
        {
            Title = title,
            Content = "Article body",
            Status = status
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<ArticleDto>())!;
    }
}

public class ArticlesApiFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _databaseRoot = new();
    private readonly string _databaseName = $"article-api-tests-{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration(configuration =>
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Database:Initialize"] = "false"
            }));
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ArticleDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<ArticleDbContext>>();
            services.AddDbContext<ArticleDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName, _databaseRoot));
        });
    }
}
