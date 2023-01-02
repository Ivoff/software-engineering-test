namespace ForumAggregator.Domain.Shared.Interfaces;

using System;

public interface IEntity
{
    Guid Id 
    {
        get;
        init;
    }
}