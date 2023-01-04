namespace ForumAggregator.Application.Services;

public interface IUserService
{
    public bool UserEmailExist(string email);
    public UserAppServiceModel? GetUser(Guid id);
    public UserAppServiceModel? GetUser(string email);
}