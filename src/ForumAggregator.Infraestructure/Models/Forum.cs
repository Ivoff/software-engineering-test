namespace ForumAggregator.Infraestructure.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class Forum
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Description { get; set; } = default!;

    public bool Deleted { get; set; }

    public DateTime CreatedAt { get; set; }

    ICollection<Moderator> Moderators { get; set; } = default!;
    
    ICollection<BlackListed> BlackList { get; set; } = default!;
}