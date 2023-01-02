namespace ForumAggregator.Application.UseCases.UserRegistration;

public interface IUserRegistrationUseCase
{
    public UserUseCaseModel Register(string name, string email, string password);
}