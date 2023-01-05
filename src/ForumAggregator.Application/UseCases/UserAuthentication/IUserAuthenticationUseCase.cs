namespace ForumAggregator.Application.UseCases;

public interface IUserAuthenticationUseCase
{
    public EntityUseCaseResult Register(string name, string email, string password);
    public EntityUseCaseResult Login(string email, string password);
}