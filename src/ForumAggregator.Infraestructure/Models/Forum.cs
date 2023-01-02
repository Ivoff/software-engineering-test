namespace ForumAggregator.Infraestructure.Models;

using System;

public class Forum
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool Deleted { get; set; }

    ICollection<Moderator> Moderators { get; set; } = default!;
    ICollection<BlackListed> BlackList { get; set; } = default!;
}