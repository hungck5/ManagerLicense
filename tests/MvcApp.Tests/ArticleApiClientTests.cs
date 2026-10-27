using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using MvcApp.Models;
using MvcApp.Services;
using Xunit;

namespace MvcApp.Tests;

public class ArticleApiClientTests
{
    [Fact]
    public async Task GetPublishedBySlugAsync_EscapesSlugAndReturnsArticle()
    {
        var handler = new StubHandler(HttpStatusCode.OK, new Article { Title = "Article", Slug = "a/b" });
        var client = new ArticleApiClient(new HttpClient(handler), Configuration("https://api.test/api"));

        var article = await client.GetPublishedBySlugAsync("a/b");

        Assert.Equal("https://api.test/api/articles/a%2Fb?publishedOnly=true", handler.RequestUri!.ToString());
        Assert.Equal("Article", article!.Title);
    }

    [Fact]
    public async Task GetPublishedBySlugAsync_ReturnsNullForNotFound()
    {
        var client = new ArticleApiClient(new HttpClient(new StubHandler(HttpStatusCode.NotFound)), Configuration("https://api.test/api"));
        Assert.Null(await client.GetPublishedBySlugAsync("missing"));
    }

    private static IConfiguration Configuration(string baseUrl) => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?> { ["ArticleApi:BaseUrl"] = baseUrl }).Build();

    private sealed class StubHandler(HttpStatusCode status, Article? article = null) : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;
            var response = new HttpResponseMessage(status);
            if (article is not null) response.Content = JsonContent.Create(article);
            return Task.FromResult(response);
        }
    }
}
