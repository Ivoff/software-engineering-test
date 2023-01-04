namespace ForumAggregator.Application.UseCases;

using AutoMapper;
using System.Text;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Application.Services;
using ForumAggregator.Domain.UserRegistry;

public class UserRegistrationUserCase : IUserRegistrationUseCase
{
    private readonly IUserRepository _user_repository;
    private readonly IUserService _user_service;
    private readonly IPasswordService _password_service;
    private readonly IMapper _mapper;

    public UserRegistrationUserCase(
        IUserRepository userRepository, 
        IUserService user_service, 
        IPasswordService password_service, 
        IMapper mapper
    )
    {
        _user_repository = userRepository;
        _user_service = user_service;
        _password_service = password_service;
        _mapper = mapper;
    }

    public UserUseCaseResult Register(string name, string email, string password)
    {
        if (_user_service.UserEmailExist(email))
            return new UserUseCaseResult(false, "User already exist.", null);

        byte[] salt;
        string hashedPassword = _password_service.HashPassword(password, out salt);
        
        User newUser = new User(name, email, hashedPassword);        

        bool result = _user_repository.Save(newUser, salt);    

        return new UserUseCaseResult(
            result, 
            result ? string.Empty : "Something wrong happened during data persistance", 
            result ? new UserUseCaseModel(newUser.Id, newUser.Name) : null
        );
    }
}