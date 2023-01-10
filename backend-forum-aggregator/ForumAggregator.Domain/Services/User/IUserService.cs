namespace ForumAggregator.Domain.Services;

using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.UserRegistry;
using ForumAggregator.Domain.ForumRegistry;
using ForumAggregator.Domain.PostRegistry;
using ForumAggregator.Domain.CommentRegistry;

public interface IUserService : IDomainService
{
    public bool IsUserNameUnique(string userName);
    public UserServiceResult DeleteUserContent(User user);
}