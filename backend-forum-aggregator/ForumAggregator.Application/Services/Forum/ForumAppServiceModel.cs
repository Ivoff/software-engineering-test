namespace ForumAggregator.Application.Services;

using System;
using System.Collections.Generic;

public class ForumAppServiceModel
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool Deleted { get; set; } = default;
    public ICollection<ModeratorAppServiceModel> Moderators { get; set; } = default!;
    public ICollection<BlackListedAppServiceModel> BlackList { get; set; } = default!;
}