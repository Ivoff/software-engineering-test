namespace ForumAggregator.Domain.Shared.Interfaces;

public interface IDomainResult<T>
{
    public T Value { get; init;}
    public string Result { get; init; }
}