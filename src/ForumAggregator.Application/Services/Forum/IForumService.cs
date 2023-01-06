namespace ForumAggregator.Application.Services;

using System;
using System.Collections.Generic;

public interface IForumService
{
    public ForumAppServiceModel? GetForum(Guid forumId);
    public ForumAppServiceModel? GetForumByName(string forumName);
    public ICollection<ForumAppServiceModel> GetAllForums();
    public ServiceResult UpdateForum(ForumAppServiceModel forum);
    public ServiceResult DeleteForum(Guid forumId);
}