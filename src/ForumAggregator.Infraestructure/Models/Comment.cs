namespace ForumAggregator.Infraestructure.Models;

using System;

public class Comment
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid ParentCommentId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = default!;
    public bool Deleted { get; set; }
}