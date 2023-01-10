namespace ForumAggregator.Infraestructure.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Moderator
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    public Guid ForumId { get; set; }
    [ForeignKey("ForumId")]
    public virtual Forum Forum { get; set; } = null!;

    public bool Deleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<ModeratorAuthority> ModeratorAuthorities { get; set; } = default!;
}