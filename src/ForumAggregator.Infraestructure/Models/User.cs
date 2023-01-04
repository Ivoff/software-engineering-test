namespace ForumAggregator.Infraestructure.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Email { get; set; } = default!;
    
    [Required]
    public string Password { get; set; } = default!;

    [Required]
    public byte[] Salt { get; set; } = default!;

    public bool Deleted { get; set; }
    
    public DateTime CreatedAt { get; set; }
}