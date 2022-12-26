using System.ComponentModel.DataAnnotations;

namespace Articles.RestApi.Models;

public record ArticleResponse
{
    [Required]
    public required string Title { get; init; }
}