namespace ForumAggregator.Infraestructure.Models;

using System;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Password{ get; set; } = default!;
    public bool Deleted { get; set; }
}