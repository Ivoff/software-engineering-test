namespace ForumAggregator.Domain.Services;

using ForumAggregator.Domain.Shared.Interfaces;
using UserRegistry;

// This is a Domain Service because is a business rule.
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public bool IsUserNameUnique(string userName)
    {
        User? user = _userRepository.GetByName(userName);
        return user == null;
    }
}