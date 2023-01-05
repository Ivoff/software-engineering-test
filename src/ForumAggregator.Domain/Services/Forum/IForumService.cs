namespace ForumAggregator.Domain.Services;

using ForumAggregator.Domain.Shared.Interfaces;

public interface IForumService : IDomainService
{
    public bool IsForumNameUnique(string forumName);
}