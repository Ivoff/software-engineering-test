namespace ForumAggregator.Infraestructure.Models;

using System;

using ForumAggregator.Domain.ForumRegistry;

public class ModeratorAuthority
{
    public Guid ModeratorId { get; set; }
    public EAuthority Authority { get; set; }
}