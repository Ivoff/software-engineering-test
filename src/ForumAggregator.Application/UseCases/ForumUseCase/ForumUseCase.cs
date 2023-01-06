using ForumAggregator.Application.UseCases;

using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.ForumRegistry;
using ForumAggregator.Application;
using System.Linq;

public class ForumUseCase : IForumUseCase
{
    private readonly IForumRepository _forumRepository;
    private readonly ForumAggregator.Domain.Services.IForumService _domainForumService;
    private readonly IAppContext _appContext;

    public ForumUseCase(
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
        ForumResult addModeratorResult = default!;
        foreach(var moderator in moderators)
        {
            addModeratorResult = newForum.AddModerator(
                _appContext.UserId,
                moderator.UserId,
                moderator.Authorities.Select(authority => (EAuthority)authority).ToList()
            );

            if (addModeratorResult.Value == false)
                break;
        }

        if (addModeratorResult.Value == false)
            return new EntityUseCaseResult(false, addModeratorResult.Result, null);

        //TODO: Add blacklsited users following Domain business rules
        ForumResult addBlackListedResult = default!;
        foreach(var blackListed in blackList)
        {
            addBlackListedResult = newForum.AddBlackListed(
                _appContext.UserId,
                blackListed.UserId,
                blackListed.CanComment,
                blackListed.CanPost
            );

            if (addBlackListedResult.Value == false)
                break;
        }

        if (addBlackListedResult.Value == false)
            return new EntityUseCaseResult(false, addBlackListedResult.Result, null);

        bool result = _forumRepository.Save(newForum);

        return new EntityUseCaseResult(
            result,
            result ? string.Empty : "Something wrong happened during data persistance",
            result ? new EntityUseCaseDto(newForum.Id, newForum.Name) : null
        );
    }

    public EntityUseCaseResult AddModerator(Guid forumId, Guid userId, ICollection<int> authorities)
    {
        var forum = _forumRepository.Get(forumId);
        if (forum == null)
            return new EntityUseCaseResult(false, "Forum does not exist.", null);
        
        var resultAddModerator = forum.AddModerator(_appContext.UserId, userId, authorities.Select(x => (EAuthority) x).ToList());
        
        if (resultAddModerator.Value == false)
            return new EntityUseCaseResult(false, resultAddModerator.Result, null);
        
        var result = _forumRepository.SaveModerator(forumId, forum.GetModeratorByUserId(userId)!);
        
        return new EntityUseCaseResult(
            result,
            result ? string.Empty : "Something wrong happened during data persistance",
            null
        );
    }

    public EntityUseCaseResult AddModerator(Guid forumId, ICollection<ModeratorUseCaseModel> moderators)
    {
        var forum = _forumRepository.Get(forumId);
        if (forum == null)
            return new EntityUseCaseResult(false, "Forum does not exist.", null);

        var results = new List<ForumResult>();
        foreach(var moderator in moderators)
        {
            var resultAddModerator = forum.AddModerator(
                _appContext.UserId, 
                moderator.UserId, 
                moderator.Authorities.Select(x => (EAuthority) x).ToList()
            );

            results.Add(resultAddModerator);
        }
        
        if (results.Any(x => x.Value == false))
        {
            var errors = string.Join("\n", results.Where(x => x.Value == false).Select(x => x.Result).ToList());
            
            return new EntityUseCaseResult(
                false, errors, null
            );
        }

        bool result = true;
        foreach(var moderator in moderators)
        {
            result = result && _forumRepository.SaveModerator(forumId, forum.GetModeratorByUserId(moderator.UserId)!);
        }
        
        var ids = string.Join(", ", results.Select(x => x.Result).ToList());
        
        return new EntityUseCaseResult(
            result,
            result ? ids : "Something wrong happened during data persistance",
            null
        );
    }

    public EntityUseCaseResult UpdateModeratorAuthorities(Guid forumId, Guid moderatorId, ICollection<int> authorities)
    {
        throw new NotImplementedException();
    }

    public EntityUseCaseResult RemoveModerator(Guid forumId, Guid moderatorId, ICollection<int> authorities)
    {
        throw new NotImplementedException();
    }

    public EntityUseCaseResult AddBlackListed(Guid forumId, Guid userId, bool canPost, bool canComment)
    {
        throw new NotImplementedException();
    }    

    public EntityUseCaseResult RemoveBlackListed(Guid forumId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public EntityUseCaseResult UpdateBlackListed(Guid forumId, Guid userId, bool canPost, bool canComment)
    {
        throw new NotImplementedException();
    }
}