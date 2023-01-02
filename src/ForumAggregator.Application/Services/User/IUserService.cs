namespace ForumAggregator.Application.Services.User;

public interface IUserService
{
    public bool UserEmailExist(string email);
}