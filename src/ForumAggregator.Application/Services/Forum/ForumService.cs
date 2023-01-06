namespace ForumAggregator.Application.Services;

using System;
using ForumAggregator.Domain.Shared.Interfaces;
using AutoMapper;
using ForumAggregator.Domain.ForumRegistry;

public class ForumService : IForumService
{
    private readonly IForumRepository _forumRepository;
    private readonly IMapper _mapper;
    private readonly IAppContext _appContext;

    public ForumService(IForumRepository forumRepository, IMapper mapper, IAppContext appContext)
    {
        _forumRepository = forumRepository;
        _mapper = mapper;
        _appContext = appContext;
    }

    public ServiceResult DeleteForum(Guid forumId)
    {
        throw new NotImplementedException();
    }

    public ForumAppServiceModel? GetForum(Guid forumId)
    {
        var forum = _forumRepository.Get(forumId);
        if (forum == null)
            return null;
        
        return _mapper.Map<ForumAppServiceModel>(forum);
    }

    public ForumAppServiceModel? GetForumByName(string forumName)
    {
        var forum = _forumRepository.GetByName(forumName);
        if (forum == null)
            return null;
        
        return _mapper.Map<ForumAppServiceModel>(forum);
    }

    public ICollection<ForumAppServiceModel> GetAllForums()
    {
        return _forumRepository.GetAll().Select(x => _mapper.Map<ForumAppServiceModel>(x)).ToList();
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
}