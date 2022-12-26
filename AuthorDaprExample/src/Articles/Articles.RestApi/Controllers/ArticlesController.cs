using System.Net.Mime;
using Articles.RestApi.Domain;
using Articles.RestApi.Models;
using Articles.RestApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Articles.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ArticlesController : ControllerBase
{
    private readonly AppDbContext context;

    public ArticlesController(AppDbContext context)
    {
        this.context = context;
    }

    [HttpGet(Name = "GetArticles")]
    public async IAsyncEnumerable<ArticleResponse> Get()
    {
        await foreach (Article article in context.Articles.AsAsyncEnumerable())
        {
            yield return new ArticleResponse
            {
                Title = article.Title
            };
        }
    }

    [HttpGet("{title}", Name = "GetArticle")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<ArticleResponse>> Get(string title)
    {
        Article? article = await context.Articles.SingleOrDefaultAsync(a => a.Title.Equals(title));
        return article is not null ? Ok(article) : NotFound();
    }

    [HttpPost(Name = "CreateArticle")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<PostCreatedResponse>> Post(CreatePostRequest request)
    {
        var article = new Article
        {
            Title = request.Title
        };
        await context.Articles.AddAsync(article);

        await context.SaveChangesAsync();

        var postCreatedResponse = new PostCreatedResponse { PostId = article.Id };
        return CreatedAtAction(nameof(Get), postCreatedResponse, article.Id.ToString());
    }
}

public record CreatePostRequest
{
    public string Title { get; set; }
}

public record PostCreatedResponse
{
    public Guid PostId { get; set; }
}