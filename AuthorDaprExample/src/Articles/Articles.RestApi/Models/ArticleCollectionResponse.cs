namespace Articles.RestApi.Models;

public record ArticleCollectionResponse
{
    public List<ArticleResponse> Articles { get; set; } = new();
}