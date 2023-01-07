namespace ForumAggregator.Application.Services;

using System;

public interface IPostService
{
    public PostAppServiceModel? GetPost(Guid postId);
    public ServiceResult UpdatePost(Guid postId, string newTitle, string newContent);
    public ServiceResult DeletePost(Guid postId);
}