namespace ForumAggregator.Domain.Shared.Interfaces;

using System;

public interface IRepository<T> where T : IEntity
{
    public T? Get(Guid id);

    public T Save(T entity);
}