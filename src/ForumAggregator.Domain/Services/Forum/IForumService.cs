namespace ForumAggregator.Domain.Services.ForumService;

using ForumAggregator.Domain.Shared.Interfaces;

public interface IForumService : IDomainService
{
    public bool IsForumNameUnique(string forumName);
}