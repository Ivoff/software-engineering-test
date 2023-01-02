namespace ForumAggregator.Domain.Services.UserService;

// This is a Domain Service because is a business rule.
public class UserService : IUserService
{
    public bool IsUserNameUnique(string userName)
    {
        return false;
    }
}