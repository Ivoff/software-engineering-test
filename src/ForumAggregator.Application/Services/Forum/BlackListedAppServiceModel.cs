namespace ForumAggregator.Application.Services;

using System;

public class BlackListed
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ForumId { get; set; }
    public bool CanComment { get; set; }
    public bool CanPost { get; set; }
    public bool Deleted { get; set; }
}