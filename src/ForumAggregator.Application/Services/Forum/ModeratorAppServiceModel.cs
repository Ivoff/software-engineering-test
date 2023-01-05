namespace ForumAggregator.Application.Services;

using System;
using System.Collections.Generic;
using ForumAggregator.Domain.ForumRegistry;

public class ModeratorAppServiceModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool Deleted { get; set; }
    public ICollection<EAuthority> Authorities { get; set; } = default!;
}