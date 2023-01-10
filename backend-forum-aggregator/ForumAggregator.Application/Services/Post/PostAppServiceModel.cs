namespace ForumAggregator.Application.Services;

public class PostAppServiceModel
{
    public Guid Id { get; set; }
    public Guid ForumId { get; set; }
    public string ForumName{ get; set; } = default!;
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = default!;
    public string Title  { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool Deleted { get; private set; } = default!;
}