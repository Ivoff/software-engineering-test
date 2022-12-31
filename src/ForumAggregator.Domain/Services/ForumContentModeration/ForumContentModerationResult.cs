namespace ForumAggregator.Domain.Services.ForumContentModerationService;

using ForumAggregator.Domain.Shared.Interfaces;

public record ForumContentModerationResult: IDomainResult<bool>
{
    public bool Value { get; init; }

    public string Result { get; init; } = default!;
}