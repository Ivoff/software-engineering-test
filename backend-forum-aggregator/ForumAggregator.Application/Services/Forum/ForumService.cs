namespace ForumAggregator.Application.Services;

using System;
using ForumAggregator.Domain.Shared.Interfaces;
using AutoMapper;
using ForumAggregator.Domain.ForumRegistry;

public class ForumService : IForumService
{
    private readonly IForumRepository _forumRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IAppContext _appContext;

    public ForumService(
        IForumRepository forumRepository, 
        IMapper mapper, 
        IAppContext appContext,
        IUserRepository userRepository
    )
    {
        _forumRepository = forumRepository;
        _mapper = mapper;
        _appContext = appContext;
        _userRepository = userRepository;
    }

    public ForumAppServiceModel? GetForum(Guid forumId)
    {
        var forum = _forumRepository.Get(forumId);
        if (forum == null)
            return null;
        
        var appForum = _mapper.Map<ForumAppServiceModel>(forum);
        appForum.OwnerName = _userRepository.Get(appForum.OwnerId)?.Name ?? string.Empty;
        return appForum;
    }

    public ForumAppServiceModel? GetForumByName(string forumName)
    {
        var forum = _forumRepository.GetByName(forumName);
        if (forum == null)
            return null;
        
        var appForum = _mapper.Map<ForumAppServiceModel>(forum);
        appForum.OwnerName = _userRepository.Get(appForum.OwnerId)?.Name ?? string.Empty;
        return appForum;
    }

    public ICollection<ForumAppServiceModel> GetAllForums()
    {
        var appForums = _forumRepository
            .GetAll()
            .Select(x => _mapper.Map<ForumAppServiceModel>(x))
            .ToList();
        
        return appForums.Select(x => {
            x.OwnerName = _userRepository.Get(x.OwnerId)?.Name ?? string.Empty;
            return x;
        }).ToList();
    }

    public ServiceResult UpdateForum(Guid forumId, string newName, string newDescription)
    {
        var domainForum = _forumRepository.Get(forumId);
        if (domainForum == null)
            return new ServiceResult(false, "Forum does not exist.");
        
        if (string.IsNullOrWhiteSpace(newName) == false)
        {
            var editResult = domainForum.EditName(_appContext.UserId, newName);
            if (editResult.Value == false)
                return new ServiceResult(false, editResult.Result);
        }
        
        if (string.IsNullOrWhiteSpace(newDescription) == false)
        {
            var editResult = domainForum.EditDescription(_appContext.UserId, newDescription);
            if (editResult.Value == false)
                return new ServiceResult(false, editResult.Result);
        }

        var result = _forumRepository.Save(domainForum);

        return new ServiceResult(result, result ? string.Empty : "Something wrong happened during data persistance");
    }

    public ICollection<ForumAppServiceModel> GetAllForumsFromUser(Guid userId)
    {
        var appForums = _forumRepository
            .GetAllFromUser(userId)
            .Select(x => _mapper.Map<ForumAppServiceModel>(x))
            .ToList();

        return appForums.Select(x => {
            x.OwnerName = _userRepository.Get(x.OwnerId)?.Name ?? string.Empty;
            return x;
        }).ToList();
    }    

    public ICollection<ForumAppServiceModel> SearchForums(string searchString)
    {
        var appForums = _forumRepository.GetLike(searchString).Select(x => _mapper.Map<ForumAppServiceModel>(x)).ToList();
        return appForums.Select(x => {
            x.OwnerName = _userRepository.Get(x.OwnerId)?.Name ?? string.Empty;
            return x;
        }).ToList();
    }
}