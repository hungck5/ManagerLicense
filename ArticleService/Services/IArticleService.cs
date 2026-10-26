using ArticleService.DTOs;

namespace ArticleService.Services;

public interface IArticleService
{
    Task<IReadOnlyList<ArticleDto>> GetAllAsync(bool publishedOnly, CancellationToken cancellationToken);
    Task<ArticleDto?> GetBySlugAsync(string slug, bool publishedOnly, CancellationToken cancellationToken);
    Task<ArticleDto> CreateAsync(SaveArticleRequest request, CancellationToken cancellationToken);
    Task<ArticleDto?> UpdateAsync(Guid id, SaveArticleRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
