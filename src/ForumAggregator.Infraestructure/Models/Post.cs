namespace ForumAggregator.Infraestructure.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Post
{
    public Guid Id { get; set; }
    
    [Required]
    [ForeignKey("Forum")]
    public Guid ForumId { get; set; }
    public virtual Forum Forum { get; set; } = null!;

    [Required]
    [ForeignKey("User")]
    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; } = null!;

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Content { get; set; } = null!;

    public bool Deleted { get; set; }

    public DateTime CreatedAt { get; set; }
}