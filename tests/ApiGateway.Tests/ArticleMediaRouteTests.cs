using System.Text.Json;
using Xunit;

namespace ApiGateway.Tests;

public class ArticleMediaRouteTests
{
    [Fact]
    public void Gateway_MapsArticleMediaToArticleCluster()
    {
        var settingsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../ApiGateway/appsettings.json"));
        using var document = JsonDocument.Parse(File.ReadAllText(settingsPath));
        var route = document.RootElement.GetProperty("ReverseProxy").GetProperty("Routes").GetProperty("article_media_route");

        Assert.Equal("article_cluster", route.GetProperty("ClusterId").GetString());
        Assert.Equal("/article-media/{**catch-all}", route.GetProperty("Match").GetProperty("Path").GetString());
    }
}
