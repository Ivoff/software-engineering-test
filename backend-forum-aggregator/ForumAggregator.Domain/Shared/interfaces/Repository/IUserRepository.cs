namespace ForumAggregator.Domain.Shared.Interfaces;

using System;
using System.Collections.Generic;

public interface IUserRepository
{
    public Domain.UserRegistry.User? Get(Guid id);
    
    public Domain.UserRegistry.User? Get(string email);
    
    public Domain.UserRegistry.User? GetByName(string name);
    
    public ICollection<Domain.UserRegistry.User> GetAll();
    
    public bool Save(Domain.UserRegistry.User entity);

    public bool Save(Domain.UserRegistry.User entity, byte[] salt);

    public byte[] GetUserSalt(Guid id);
}