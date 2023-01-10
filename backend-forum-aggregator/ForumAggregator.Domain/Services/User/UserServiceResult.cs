namespace ForumAggregator.Domain.Services;

using ForumAggregator.Domain.Shared.Interfaces;

public record UserServiceResult: IDomainResult<bool>
{
    public bool Value { get; init; }

    public string Result { get; init; } = default!;
}