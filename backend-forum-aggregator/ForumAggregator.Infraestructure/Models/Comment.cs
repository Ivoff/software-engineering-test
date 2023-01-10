namespace ForumAggregator.Infraestructure.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Comment
{
    public Guid Id { get; set; }

    [Required]
    [ForeignKey("Post")]
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("Comment")]
    public Guid? ParentCommentId { get; set; }
    public virtual Comment ParentComment { get; set; } = null!;

    [Required]
    [ForeignKey("User")]
    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; } = null!;

    [Required]
    public string Content { get; set; } = default!;
    
    public bool Deleted { get; set; }

    public DateTime CreatedAt { get; set; }
}