namespace ForumAggregator.Domain.ForumRegistry;

using ForumAggregator.Domain.Shared.Interfaces;

public record BlackListedResult: IDomainResult<bool>
{
    public bool Value { get; init; }
    
    public string Result { get; init; } = default!;
}