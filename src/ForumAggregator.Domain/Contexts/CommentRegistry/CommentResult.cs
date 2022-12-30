namespace ForumAggregator.Domain.CommentRegistry;

using ForumAggregator.Domain.Shared.Interfaces;

public record CommentResult : IDomainResult<bool>
{
    public bool Value { get; init; }
    public string Result { get; init; } = default!;
    public Comment? Comment { get; init; }
}