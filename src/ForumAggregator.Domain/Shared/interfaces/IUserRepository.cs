namespace ForumAggregator.Domain.Shared.Interfaces;

using System;

public interface IUserRepository
{
    public Domain.UserRegistry.User? Get(Guid id);
    
    public Domain.UserRegistry.User? Get(string email);
    
    public bool Save(Domain.UserRegistry.User entity);

    public bool Save(Domain.UserRegistry.User entity, byte[] salt);
}