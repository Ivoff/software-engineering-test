namespace ForumAggregator.Application.Services;

using System;

public interface IForumService
{
    public ForumAppServiceModel? GetForum(Guid forumId);
    public ForumAppServiceModel? GetForumByName(string forumName);
    // public ServiceResult CreateForum(ForumAppServiceModel forum);
    public ServiceResult UpdateForum(ForumAppServiceModel forum);
    public ServiceResult DeleteForum(Guid forumId);
}