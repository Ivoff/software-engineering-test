namespace ForumAggregator.Infraestructure.Repository;

using ForumAggregator.Domain.Shared.Interfaces;

public class Repository<T>: IRepository<T> where T: IEntity
{
    public Repository()
    {

    }

    public T? Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public T Save(T entity)
    {
        throw new NotImplementedException();
    }
}