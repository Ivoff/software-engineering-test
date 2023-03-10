namespace ForumAggregator.Infraestructure.Repository;

using System;
using AutoMapper;
using System.Linq;
using ForumAggregator.Infraestructure.DbContext;
using ForumAggregator.Domain.Shared.Interfaces;

public class UserRepository: IUserRepository
{
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public UserRepository(DatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Domain.UserRegistry.User? Get(Guid id)
    {
        Infraestructure.Models.User? infraUser = _dbContext.Users
            .Where(x => x.Id == id && !x.Deleted)
            .FirstOrDefault();

        if (infraUser == null)
            return null;
        return _mapper.Map<Domain.UserRegistry.User>(infraUser);
    }

    public Domain.UserRegistry.User? Get(string email)
    {
        Infraestructure.Models.User? infraUser = _dbContext.Users
            .Where(user => user.Email == email && !user.Deleted)
            .FirstOrDefault();

        if (infraUser == null)
            return null;
        return _mapper.Map<Domain.UserRegistry.User>(infraUser);
    }

    public Domain.UserRegistry.User? GetByName(string name)
    {
        Infraestructure.Models.User? infraUser = _dbContext.Users
            .Where(user => user.Name == name && !user.Deleted)
            .FirstOrDefault();
            
        if (infraUser == null)
            return null;
        return _mapper.Map<Domain.UserRegistry.User>(infraUser);
    }

    public bool Save(Domain.UserRegistry.User entity)
    {
        var newUser = _mapper.Map<Infraestructure.Models.User>(entity);
        
        if (_dbContext.Users.Where(user => user.Id == newUser.Id).Count() == 0)
        {
            _dbContext.Add(newUser);
        }
        else
        {
            var currUser = _dbContext.Users.Where(user =>user.Id == newUser.Id).First();
            
            currUser.Name = newUser.Name;
            currUser.Email = newUser.Email;
            currUser.Deleted = newUser.Deleted;
            
            if (currUser.Password != newUser.Password)
            {
                throw new InvalidOperationException("Do not use this function to update Passwords");
            }
            
            _dbContext.Update(currUser);
        }

        return _dbContext.SaveChanges() > 0;
    }

    public bool Save(Domain.UserRegistry.User entity, byte[] salt)
    {
        var newUser = _mapper.Map<Infraestructure.Models.User>(entity);
        newUser.Salt = salt;

        if (_dbContext.Users.Where(user => user.Id == newUser.Id).Count() == 0)
        {
            _dbContext.Users.Add(newUser);
        }
        else 
        {
            var currUser = _dbContext.Users.Where(user =>user.Id == newUser.Id).First();
            
            currUser.Name = newUser.Name;
            currUser.Email = newUser.Email;
            currUser.Password = newUser.Password;
            currUser.Salt = newUser.Salt;
            currUser.Deleted = newUser.Deleted;            

            _dbContext.Users.Update(currUser);
        }

        return _dbContext.SaveChanges() > 0;
    }

    public byte[] GetUserSalt(Guid id)
    {
        var user = _dbContext.Users.First(user => user.Id == id);
        return user.Salt;
    }

    public ICollection<Domain.UserRegistry.User> GetAll()
    {
        return _dbContext.Users.Select(
            user => _mapper.Map<Domain.UserRegistry.User>(user)
        ).ToList();
    }
}