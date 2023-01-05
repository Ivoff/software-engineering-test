using ForumAggregator.Application.UseCases;

using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.ForumRegistry;
using ForumAggregator.Application;
using System.Linq;

public class ForumCreationUseCase : IForumCreationUseCase
{
    private readonly IForumRepository _forumRepository;
    private readonly ForumAggregator.Domain.Services.IForumService _domainForumService;
    private readonly IAppContext _appContext;

    public ForumCreationUseCase(
        IForumRepository forumRepository,
        ForumAggregator.Domain.Services.IForumService domainForumService,
        IAppContext appContext
    )
    {
        _forumRepository = forumRepository;
        _domainForumService = domainForumService;
        _appContext = appContext;
    }

    public EntityUseCaseResult Create(
        string name, 
        string description, 
        ICollection<ModeratorUseCaseModel> moderators, 
        ICollection<BlackListedUseCaseModel> blackList
    )
    {
        // TODO: Check if forum name is unique
        if (_domainForumService.IsForumNameUnique(name) == false)
            return new EntityUseCaseResult(false, "Forum Name already taken.", null);
        
        // TODO: Create forum following Domain business rules
        Forum newForum = new Forum(_appContext.UserId, name, description);

        // TODO: Add moderators following Domain business rules
        foreach(var moderator in moderators)
        {
            newForum.AddModerator(
                _appContext.UserId,
                moderator.UserId,
                moderator.Authorities.Select(authority => (EAuthority)authority).ToList()
            );
        }

        //TODO: Add blacklsited users following Domain business rules
        foreach(var blackListed in blackList)
        {
            newForum.AddBlackListed(
                _appContext.UserId,
                blackListed.UserId,
                blackListed.CanComment,
                blackListed.CanPost
            );
        }

        bool result = _forumRepository.Save(newForum);

        return new EntityUseCaseResult(
            result,
            result ? string.Empty : "Something wrong happened during data persistance",
            result ? new EntityUseCaseDto(newForum.Id, newForum.Name) : null
        );
    }
}