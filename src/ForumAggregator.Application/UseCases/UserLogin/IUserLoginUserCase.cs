namespace ForumAggregator.Application.UseCases;

public interface IUserLoginUseCase
{
    public UserUseCaseResult Login(string email, string password);
}