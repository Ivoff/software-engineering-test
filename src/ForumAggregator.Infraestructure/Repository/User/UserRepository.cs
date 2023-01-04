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
        Infraestructure.Models.User? infraUser = _dbContext.Users.Find(id);
        if (infraUser == null)
            return null;
        return _mapper.Map<Domain.UserRegistry.User>(infraUser);
    }

    public Domain.UserRegistry.User? Get(string email)
    {
        Infraestructure.Models.User? infraUser = _dbContext.Users.Where(user => user.Email == email).FirstOrDefault();
        if (infraUser == null)
            return null;
        return _mapper.Map<Domain.UserRegistry.User>(infraUser);
    }

    public bool Save(Domain.UserRegistry.User entity)
    {
        Infraestructure.Models.User newUser = _mapper.Map<Infraestructure.Models.User>(entity);
        
        if (_dbContext.Users.Where(user => user.Id == newUser.Id).Count() == 0)
        {
            _dbContext.Add(newUser);
        }
        else
        {
            _dbContext.Update(newUser);
        }

        return _dbContext.SaveChanges() > 0;
    }

    public bool Save(Domain.UserRegistry.User entity, byte[] salt)
    {
        Infraestructure.Models.User newUser = _mapper.Map<Infraestructure.Models.User>(entity);
        newUser.Salt = salt;

        if (_dbContext.Users.Where(user => user.Id == newUser.Id).Count() == 0)
        {
            _dbContext.Add(newUser);
        }
        else 
        {
            _dbContext.Update(newUser);
        }

        return _dbContext.SaveChanges() > 0;
    }
}