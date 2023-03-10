namespace ForumAggregator.Domain.Shared.Interfaces;

using System;
using System.Collections.Generic;

public interface IPostRepository
{
    public ForumAggregator.Domain.PostRegistry.Post? Get(Guid postId);
    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetAllFromForum(Guid forumId);
    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetAllFromUser(Guid userId);
    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetRecent(int count);
    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetAll();
    public bool Save(ForumAggregator.Domain.PostRegistry.Post post);
}