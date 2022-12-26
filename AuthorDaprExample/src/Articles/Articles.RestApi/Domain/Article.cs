namespace Articles.RestApi.Domain;

public class Article
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; set; }
}