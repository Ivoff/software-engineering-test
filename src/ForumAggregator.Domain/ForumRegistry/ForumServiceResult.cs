namespace ForumAggregator.Domain.Generic.ForumRegistry;

using ForumAggregator.Domain.Shared.Interfaces;

public record ForumServiceResult : IDomainResult<bool>
{
    public bool Value { get; init; }
    public string Result { get; init; } = default!;
}