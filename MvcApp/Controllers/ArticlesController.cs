using Microsoft.AspNetCore.Mvc;
using MvcApp.Services;

namespace MvcApp.Controllers;

[Route("bai-viet")]
public class ArticlesController(ArticleApiClient articleApiClient, ILogger<ArticlesController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            var articles = await articleApiClient.GetPublishedAsync(cancellationToken);
            return View(articles);
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Cannot reach the article API through ApiGateway.");
            return View("ServiceUnavailable");
        }
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Detail(string slug, CancellationToken cancellationToken)
    {
        try
        {
            var article = await articleApiClient.GetPublishedBySlugAsync(slug, cancellationToken);
            return article is null ? NotFound() : View(article);
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Cannot reach the article API through ApiGateway.");
            return View("ServiceUnavailable");
        }
    }
}
