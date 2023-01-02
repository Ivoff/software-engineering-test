namespace ForumAggregator.Infraestructure.Models;

using System;

public class Moderator
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ForumId { get; set; }
    public bool Deleted { get; set; }
    
    ICollection<ModeratorAuthority> ModeratorAuthorities { get; set; } = default!;
}