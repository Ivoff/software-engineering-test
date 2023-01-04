namespace ForumAggregator.Application.Services;

using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.UserRegistry;

public class UserService: IUserService
{
    private readonly IUserRepository _user_repository;

    public UserService(IUserRepository userRepository)
    {
        _user_repository = userRepository;
    }

    public bool UserEmailExist(string email)
    {
        User? userExit = _user_repository.Get(email);
        return userExit != null;
    }
}