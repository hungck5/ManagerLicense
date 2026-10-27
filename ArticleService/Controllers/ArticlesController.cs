using ArticleService.DTOs;
using ArticleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticlesController(IArticleService articles) : ControllerBase
{
    private const long MaxImageSize = 10 * 1024 * 1024;
    private static readonly IReadOnlyDictionary<string, string> ImageExtensions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["image/jpeg"] = ".jpg", ["image/png"] = ".png", ["image/gif"] = ".gif", ["image/webp"] = ".webp"
    };

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

    [HttpPost("images")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<object>> UploadImage(IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0) return BadRequest(new { message = "Vui lòng chọn ảnh để tải lên." });
        if (file.Length > MaxImageSize) return BadRequest(new { message = "Ảnh không được vượt quá 10 MB." });
        if (!ImageExtensions.TryGetValue(file.ContentType, out var extension))
            return BadRequest(new { message = "Chỉ hỗ trợ ảnh JPEG, PNG, GIF hoặc WebP." });

        var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "articles");
        Directory.CreateDirectory(directory);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        await using (var stream = System.IO.File.Create(Path.Combine(directory, fileName)))
            await file.CopyToAsync(stream, cancellationToken);

        var mediaPath = $"/article-media/articles/{fileName}";
        return Ok(new { url = mediaPath });
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
