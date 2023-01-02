namespace ForumAggregator.Domain.Services.UserService;

using ForumAggregator.Domain.Shared.Interfaces;

public interface IUserService : IDomainService
{
    public bool IsUserNameUnique(string userName);
}