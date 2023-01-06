namespace ForumAggregator.Application.UseCases;

using System.Collections.Generic;

public interface IForumUseCase
{
    public EntityUseCaseResult Create(
        string name, 
        string description, 
        ICollection<ModeratorUseCaseModel> moderators, 
        ICollection<BlackListedUseCaseModel> blackList
    );

    public EntityUseCaseResult AddModerator(Guid forumId, Guid userId, ICollection<int> authorities);
    
    public EntityUseCaseResult AddModerator(Guid forumId, ICollection<ModeratorUseCaseModel> moderators);

    public EntityUseCaseResult UpdateModeratorAuthorities(Guid forumId, Guid moderatorId, ICollection<int> authorities);

    public EntityUseCaseResult RemoveModerator(Guid forumId, Guid moderatorId, ICollection<int> authorities);

    public EntityUseCaseResult AddBlackListed(Guid forumId, Guid userId, bool canPost, bool canComment);

    public EntityUseCaseResult UpdateBlackListed(Guid forumId, Guid userId, bool canPost, bool canComment);

    public EntityUseCaseResult RemoveBlackListed(Guid forumId, Guid userId);
}