namespace ForumAggregator.Domain.Shared.Interfaces;

using System;

public interface IUserRepository
{
    public Domain.UserRegistry.User? Get(Guid id);
    
    public Domain.UserRegistry.User? Get(string email);
    public Domain.UserRegistry.User? GetByName(string name);
    
    public bool Save(Domain.UserRegistry.User entity);

    public bool Save(Domain.UserRegistry.User entity, byte[] salt);

    public byte[] GetUserSalt(Guid id);
}