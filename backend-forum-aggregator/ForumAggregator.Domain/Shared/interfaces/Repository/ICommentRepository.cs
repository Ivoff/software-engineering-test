namespace ForumAggregator.Domain.Shared.Interfaces;

using System;
using System.Collections.Generic;

public interface ICommentRepository
{
    public ForumAggregator.Domain.CommentRegistry.Comment? Get(Guid postId);
    public ICollection<ForumAggregator.Domain.CommentRegistry.Comment> GetAllFromForum(Guid forumId);
    public ICollection<ForumAggregator.Domain.CommentRegistry.Comment> GetAllFromUser(Guid userId);
    public ICollection<ForumAggregator.Domain.CommentRegistry.Comment> GetAll();
    public bool Save(ForumAggregator.Domain.CommentRegistry.Comment comment);
}