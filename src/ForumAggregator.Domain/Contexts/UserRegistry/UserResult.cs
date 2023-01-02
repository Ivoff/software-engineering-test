namespace ForumAggregator.Domain.UserRegistry;

using ForumAggregator.Domain.Shared.Interfaces;

public record UserResult : IDomainResult<bool>
{
    public bool Value { get; init; }
    public string Result { get; init; } = default!;
}