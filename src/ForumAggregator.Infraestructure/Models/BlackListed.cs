namespace ForumAggregator.Infraestructure.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BlackListed
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    public Guid ForumId { get; set; }
    [ForeignKey("ForumId")]
    public virtual Forum Forum { get; set; } = null!;

    [Required]
    public bool CanPost { get; set; }

    [Required]
    public bool CanComment { get; set; }

    public bool Deleted { get; set; }

    public DateTime CreatedAt { get; set; }
}