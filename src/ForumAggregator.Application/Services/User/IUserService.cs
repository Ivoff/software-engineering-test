namespace ForumAggregator.Application.Services;

public interface IUserService
{
    public bool UserEmailExist(string email);
}