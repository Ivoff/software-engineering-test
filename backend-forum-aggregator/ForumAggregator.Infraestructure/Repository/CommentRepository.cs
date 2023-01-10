namespace ForumAggregator.Infraestructure.Repository;

using System;
using System.Collections.Generic;
using AutoMapper;

using ForumAggregator.Domain.CommentRegistry;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Infraestructure.DbContext;

public class CommentRepository : ICommentRepository
{
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public CommentRepository(DatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public ForumAggregator.Domain.CommentRegistry.Comment? Get(Guid postId)
    {
        throw new NotImplementedException();
    }

    public ICollection<ForumAggregator.Domain.CommentRegistry.Comment> GetAll()
    {
        throw new NotImplementedException();
    }

    public ICollection<ForumAggregator.Domain.CommentRegistry.Comment> GetAllFromForum(Guid forumId)
    {
        throw new NotImplementedException();
    }

    public ICollection<ForumAggregator.Domain.CommentRegistry.Comment> GetAllFromUser(Guid userId)
    {
        throw new NotImplementedException();
    }

    public bool Save(ForumAggregator.Domain.CommentRegistry.Comment comment)
    {
        var commentExist = _dbContext.Comments.FirstOrDefault(x => x.Id == comment.Id);
        if (commentExist == null)
        {
            var newComment = _mapper.Map<ForumAggregator.Infraestructure.Models.Comment>(comment);
            _dbContext.Comments.Add(newComment);
        }
        else
        {
            commentExist.Content = comment.Content;
            commentExist.Deleted = comment.Deleted;

            _dbContext.Comments.Update(commentExist);
        }

        return _dbContext.SaveChanges() > 0;
    }
}