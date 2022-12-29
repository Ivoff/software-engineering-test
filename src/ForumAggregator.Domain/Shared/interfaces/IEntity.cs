namespace ForumAggregator.Domain.Shared.Interfaces;

using System;

interface IEntity
{
    Guid Id 
    {
        get;
        init;
    }
}