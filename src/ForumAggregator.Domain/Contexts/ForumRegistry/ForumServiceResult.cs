namespace ForumAggregator.Domain.ForumRegistry;

using ForumAggregator.Domain.Shared.Interfaces;

public record ForumResult : IDomainResult<bool>
{
    public bool Value { get; init; }
    public string Result { get; init; } = default!;
}