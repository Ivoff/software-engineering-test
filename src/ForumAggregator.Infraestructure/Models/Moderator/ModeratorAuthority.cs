namespace ForumAggregator.Infraestructure.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ForumAggregator.Domain.ForumRegistry;

public class ModeratorAuthority
{
    public Guid Id{ get; set; }

    public Guid ModeratorId { get; set; }
    [ForeignKey("ModeratorId")]
    public virtual Moderator Moderator { get; set; } = null!;

    [Required]
    public EAuthority Authority { get; set; }
}