using ForumAggregator.Domain.Shared.Interfaces;

namespace ForumAggregator.Domain.CommentRegistry;

public record CommentServiceResult : IDomainResult<bool>
{
    public bool Value { get; init; } = default!;
    public string Result { get; init; } = default!;
    public Comment? Comment { get; init; }
}