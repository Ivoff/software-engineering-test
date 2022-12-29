namespace ForumAggregator.Domain.CommentRegistry;

using ForumAggregator.Domain.Shared.Interfaces;

public record CommentServiceResult : IDomainResult<bool>
{
    public bool Value { get; init; } = default!;
    public string Result { get; init; } = default!;
    public Comment? Comment { get; init; }
}