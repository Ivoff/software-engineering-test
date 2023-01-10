namespace ForumAggregator.Application.Services;

using System;
using System.Collections.Generic;

public interface IForumService
{
    public ForumAppServiceModel? GetForum(Guid forumId);
    public ForumAppServiceModel? GetForumByName(string forumName);
    public ICollection<ForumAppServiceModel> GetAllForums();
    public ICollection<ForumAppServiceModel> GetAllForumsFromUser(Guid userId);
    public ICollection<ForumAppServiceModel> SearchForums(string searchString);
    public ServiceResult UpdateForum(Guid forumId, string newName, string newDescription);
}