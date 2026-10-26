using ArticleService.Models;
using System.ComponentModel.DataAnnotations;

namespace ArticleService.DTOs;

public class SaveArticleRequest
{
    [Required, StringLength(200)] public string Title { get; set; } = string.Empty;
    [StringLength(220)] public string? Slug { get; set; }
    [StringLength(500)] public string? Excerpt { get; set; }
    [Required] public string Content { get; set; } = string.Empty;
    [StringLength(60)] public string? MetaTitle { get; set; }
    [StringLength(160)] public string? MetaDescription { get; set; }
    [StringLength(500)] public string? CanonicalUrl { get; set; }
    [StringLength(100)] public string? Author { get; set; }
    public ArticleStatus Status { get; set; } = ArticleStatus.Draft;
}
