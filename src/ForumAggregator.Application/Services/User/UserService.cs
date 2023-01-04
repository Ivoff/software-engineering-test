namespace ForumAggregator.Application.Services;

using AutoMapper;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.UserRegistry;

public class UserService: IUserService
{
    private readonly IUserRepository _user_repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _user_repository = userRepository;
        _mapper = mapper;
    }

    public bool UserEmailExist(string email)
    {
        User? userExit = _user_repository.Get(email);
        return userExit != null;
    }

    public UserAppServiceModel? GetUser(Guid id)
    {
        User? user = _user_repository.Get(id);
        
        if (user == null)
            return null;
        
        return _mapper.Map<UserAppServiceModel>(user);
    }

    public UserAppServiceModel? GetUser(string email)
    {
        User? user = _user_repository.Get(email);
        
        if (user == null)
            return null;
        
        return _mapper.Map<UserAppServiceModel>(user);       
    }
}