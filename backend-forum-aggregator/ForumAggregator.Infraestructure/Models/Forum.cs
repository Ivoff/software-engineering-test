namespace ForumAggregator.Infraestructure.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Forum
{
    public Guid Id { get; set; }

    [ForeignKey("User")]
    public Guid OwnerId { get; set; }
    public virtual User Owner { get; set; } = default!;

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Description { get; set; } = default!;

    public bool Deleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Moderator> Moderators { get; set; } = default!;
    
    public ICollection<BlackListed> BlackList { get; set; } = default!;
}