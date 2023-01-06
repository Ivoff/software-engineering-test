namespace ForumAggregator.Application.Services;

using System;
using ForumAggregator.Domain.Shared.Interfaces;
using AutoMapper;

public class ForumService : IForumService
{
    private readonly IForumRepository _forumRepository;
    private readonly IMapper _mapper;

    public ForumService(IForumRepository forumRepository, IMapper mapper)
    {
        _forumRepository = forumRepository;
        _mapper = mapper;
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

    public ServiceResult UpdateForum(ForumAppServiceModel forum)
    {
        throw new NotImplementedException();
    }
}