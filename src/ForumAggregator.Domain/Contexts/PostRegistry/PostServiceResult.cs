namespace ForumAggregator.Domain.PostRegistry;

using ForumAggregator.Domain.Shared.Interfaces;

public record PostServiceResult: IDomainResult<bool>
{
    public bool Value { get; init; }
    public string Result { get; init; } = default!;

    public Post? Post { get; init; }
}