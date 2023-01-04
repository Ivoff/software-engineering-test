namespace ForumAggregator.Domain.Services;

using ForumAggregator.Domain.Shared.Interfaces;

public interface IUserService : IDomainService
{
    public bool IsUserNameUnique(string userName);
}