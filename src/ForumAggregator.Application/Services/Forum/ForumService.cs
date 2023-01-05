namespace ForumAggregator.Application.Services;

using System;
using ForumAggregator.Domain.Shared.Interfaces;

public class ForumService : IForumService
{
    private readonly IForumRepository _forumRepository;

    public ForumService(IForumRepository forumRepository)
    {
        _forumRepository = forumRepository;
    }

    // public ServiceResult CreateForum(ForumAppServiceModel forum)
    // {
    //     throw new NotImplementedException();
    // }

    public ServiceResult DeleteForum(Guid forumId)
    {
        throw new NotImplementedException();
    }

    public ForumAppServiceModel? GetForum(Guid forumId)
    {
        throw new NotImplementedException();
    }

    public ForumAppServiceModel? GetForumByName(string forumName)
    {
        throw new NotImplementedException();
    }

    public ServiceResult UpdateForum(ForumAppServiceModel forum)
    {
        throw new NotImplementedException();
    }
}