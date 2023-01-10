namespace ForumAggregator.Application.UseCases;

using System.Collections.Generic;
using ForumAggregator.Application.Services;

public interface IForumUseCase
{
    public EntityUseCaseResult Create(
        string name, 
        string description, 
        ICollection<ModeratorUseCaseModel> moderators, 
        ICollection<BlackListedUseCaseModel> blackList
    );

    public EntityUseCaseResult Delete(Guid forumId);

    public EntityUseCaseResult AddModerator(Guid forumId, Guid userId, ICollection<int> authorities);
    public EntityUseCaseResult AddModerator(Guid forumId, ICollection<ModeratorUseCaseModel> moderators);
    public EntityUseCaseResult UpdateModeratorAuthorities(Guid forumId, Guid moderatorId, ICollection<int> authorities);
    public EntityUseCaseResult UpdateModeratorAuthorities(Guid forumId, ICollection<ModeratorUseCaseModel> moderators);
    public EntityUseCaseResult RemoveModerator(Guid forumId, Guid moderatorId);
    public EntityUseCaseResult RemoveModerator(Guid forumId, ICollection<ModeratorUseCaseModel> moderators);
    public ModeratorAppServiceModel? GetModerator(Guid forumId, Guid moderatorId);
    public ModeratorAppServiceModel? GetModeratorByUserId(Guid forumId, Guid userId);
    public ICollection<ModeratorAppServiceModel> GetAllModerators(Guid forumId);
    public BlackListedAppServiceModel? GetBlackListed(Guid forumId, Guid blackListedId);
    public BlackListedAppServiceModel? GetBlackListedByUserId(Guid forumId, Guid userId);
    public ICollection<BlackListedAppServiceModel> GetAllBlackListed(Guid forumId);
    public EntityUseCaseResult AddBlackListed(Guid forumId, Guid userId, bool canPost, bool canComment);
    public EntityUseCaseResult AddBlackListed(Guid forumId, ICollection<BlackListedUseCaseModel> blackListedUsers);
    public EntityUseCaseResult UpdateBlackListed(Guid forumId, Guid userId, bool? canPost, bool? canComment);
    public EntityUseCaseResult UpdateBlackListed(Guid forumId, ICollection<BlackListedUseCaseModel> blackListedUsers);
    public EntityUseCaseResult RemoveBlackListed(Guid forumId, Guid userId);
    public EntityUseCaseResult RemoveBlackListed(Guid forumId, ICollection<BlackListedUseCaseModel> blackListedUsers);
}