namespace ForumAggregator.Application.UseCases;

using AutoMapper;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Application.Services;
using ForumAggregator.Domain.UserRegistry;

public class UserAuthenticationUserCase : IUserAuthenticationUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly Domain.Services.IUserService _domainUserService;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;

    public UserAuthenticationUserCase(
        IUserRepository userRepository, 
        IUserService user_service, 
        IPasswordService password_service, 
        IMapper mapper,
        Domain.Services.IUserService domainUserService
    )
    {
        _userRepository = userRepository;
        _userService = user_service;
        _passwordService = password_service;
        _mapper = mapper;
        _domainUserService = domainUserService;
    }

    public UserUseCaseResult Register(string name, string email, string password)
    {
        if (_userService.UserEmailExist(email))
            return new UserUseCaseResult(false, "User already registered.", null);
        
        if (!_domainUserService.IsUserNameUnique(name))
            return new UserUseCaseResult(false, "UserName already taken.", null);

        byte[] salt;
        string hashedPassword = _passwordService.HashPassword(password, out salt);
        
        User newUser = new User(name, email, hashedPassword);        

        bool result = _userRepository.Save(newUser, salt);    

        return new UserUseCaseResult(
            result, 
            result ? string.Empty : "Something wrong happened during data persistance", 
            result ? new UserUseCaseModel(newUser.Id, newUser.Name) : null
        );
    }

    public UserUseCaseResult Login(string email, string password)
    {
        User? userExist = _userRepository.Get(email);
        
        if (userExist == null)
        {
            return new UserUseCaseResult(false, "User not registered.", null);
        }
        
        User user = (User) userExist;
        byte[] salt = _userRepository.GetUserSalt(user.Id);

        bool result = _passwordService.CheckPassword(password, user.Password, salt);

        return new UserUseCaseResult(
            result,
            result ? string.Empty : "E-mail or/and Password incorrect.",
            result ? new UserUseCaseModel(user.Id, user.Name) : null
        );
    }
}