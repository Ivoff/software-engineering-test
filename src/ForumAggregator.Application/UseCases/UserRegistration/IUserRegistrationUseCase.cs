namespace ForumAggregator.Application.UseCases;

public interface IUserRegistrationUseCase
{
    public UserUseCaseResult Register(string name, string email, string password);
}