namespace ForumAggregator.Application.UseCases;

public interface IUserAuthenticationUseCase
{
    public UserUseCaseResult Register(string name, string email, string password);
    public UserUseCaseResult Login(string email, string password);
}