using ArticleService.DTOs;
using ArticleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticlesController(IArticleService articles) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<ArticleDto>> GetAll([FromQuery] bool publishedOnly = false, CancellationToken cancellationToken = default) =>
        articles.GetAllAsync(publishedOnly, cancellationToken);

    [HttpGet("{slug}")]
    public async Task<ActionResult<ArticleDto>> GetBySlug(string slug, [FromQuery] bool publishedOnly = false, CancellationToken cancellationToken = default)
    {
        var article = await articles.GetBySlugAsync(slug, publishedOnly, cancellationToken);
        return article is null ? NotFound() : Ok(article);
    }

    [HttpPost]
    public async Task<ActionResult<ArticleDto>> Create(SaveArticleRequest request, CancellationToken cancellationToken)
    {
        var article = await articles.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetBySlug), new { slug = article.Slug }, article);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ArticleDto>> Update(Guid id, SaveArticleRequest request, CancellationToken cancellationToken)
    {
        var article = await articles.UpdateAsync(id, request, cancellationToken);
        return article is null ? NotFound() : Ok(article);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken) =>
        await articles.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
