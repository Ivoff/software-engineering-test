namespace ForumAggregator.Domain.Shared.Interfaces;

using System;

public interface IAggregateRoot
{
    Guid Id { get; init; }
}