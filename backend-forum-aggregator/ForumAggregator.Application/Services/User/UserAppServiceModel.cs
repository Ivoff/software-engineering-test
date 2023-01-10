namespace ForumAggregator.Application.Services;

using System;

public class UserAppServiceModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public bool Deleted { get; init; }

    public UserAppServiceModel(){}
}