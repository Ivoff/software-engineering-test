namespace ForumAggregator.Application.UseCases;

using ForumAggregator.Application.Services;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.UserRegistry;

public class UserLoginUseCase : IUserLoginUseCase
{
    private readonly IUserService _user_service;
    private readonly IPasswordService _password_service;
    private readonly IUserRepository _user_repository;

    public UserLoginUseCase(IUserService user_service, IPasswordService password_service, IUserRepository user_repository)
    {
        _user_service = user_service;
        _password_service = password_service;
        _user_repository = user_repository;
    }

    public UserUseCaseResult Login(string email, string password)
    {
        User? userExist = _user_repository.Get(email);
        
        if (userExist == null)
        {
            return new UserUseCaseResult(false, "User does not exist.", null);
        }
        
        User user = (User) userExist;
        byte[] salt = _user_repository.GetUserSalt(user.Id);

        bool result = _password_service.CheckPassword(password, user.Password, salt);

        return new UserUseCaseResult(
            result,
            result ? string.Empty : "E-mail or/and Password incorrect.",
            result ? new UserUseCaseModel(user.Id, user.Name) : null
        );
    }
}