namespace ForumAggregator.Infraestructure.Repository;

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using ForumAggregator.Domain.PostRegistry;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Infraestructure.DbContext;

public class PostRepository : IPostRepository
{
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public PostRepository (DatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public ForumAggregator.Domain.PostRegistry.Post Get(Guid postId)
    {
        throw new NotImplementedException();
    }

    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetAllFromForum(Guid forumId)
    {
        throw new NotImplementedException();
    }

    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetAllFromUser(Guid userId)
    {
        throw new NotImplementedException();
    }

    public bool Save(ForumAggregator.Domain.PostRegistry.Post post)
    {
        var postExist = _dbContext.Posts.FirstOrDefault(x => x.Id == post.Id);
        if (postExist == null)
        {
            var newPost = _mapper.Map<Models.Post>(post);
            _dbContext.Posts.Add(newPost);
        }
        else
        {
            postExist.Title = post.Title;
            postExist.Content = post.Content;
            postExist.Deleted = post.Deleted;

            _dbContext.Posts.Update(postExist);
        }

        return _dbContext.SaveChanges() > 0;
    }
}