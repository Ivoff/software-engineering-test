namespace ForumAggregator.Application.Services;

using System;

public interface IPostService
{
    public PostAppServiceModel? GetPost(Guid postId);
    public ICollection<PostAppServiceModel> GetAllPosts();
    public ICollection<PostAppServiceModel> GetRecentPosts(int count);
    public ICollection<PostAppServiceModel> GetAllPostsFromUser(Guid userId);
    public ICollection<PostAppServiceModel> GetAllPostsFromForum(Guid forumId);
    public ServiceResult UpdatePost(Guid postId, string newTitle, string newContent);
    public ServiceResult DeletePost(Guid postId);
}