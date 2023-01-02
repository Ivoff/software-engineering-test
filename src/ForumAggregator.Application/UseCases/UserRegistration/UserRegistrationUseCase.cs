namespace ForumAggregator.Application.UseCases.UserRegistration;

using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.UserRegistry;

public class UserRegistrationUserCase : IUserRegistrationUseCase
{
    private readonly IRepository<User> _user_repository;

    public UserRegistrationUserCase(IRepository<User> userRepository)
    {
        _user_repository = userRepository;
    }

    public UserUseCaseModel Register(string name, string email, string password)
    {
        return new UserUseCaseModel(Guid.Empty, string.Empty);
    }
}