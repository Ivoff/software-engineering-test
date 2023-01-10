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

    public PostRepository(DatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public ForumAggregator.Domain.PostRegistry.Post? Get(Guid postId)
    {
        var post = _dbContext.Posts.FirstOrDefault(x => x.Id == postId && !x.Deleted);
        if (post == null)
            return null;

        // For some reason AutoMapper is not mapping AuthorId to new PostAuthor(AuthorId, false)
        // Therefore, this here, do not work.
        // return _mapper.Map<ForumAggregator.Domain.PostRegistry.Post>(post);

        return ForumAggregator.Domain.PostRegistry.Post.Load(
            post.Id, 
            post.ForumId, 
            post.Title, 
            post.Content, 
            post.Deleted,
            new ForumAggregator.Domain.PostRegistry.PostAuthor(post.AuthorId, false)
        );
    }

    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetAllFromForum(Guid forumId)
    {
        var posts = _dbContext.Posts.Where(x => x.ForumId == forumId && !x.Deleted).OrderByDescending(x => x.CreatedAt).ToList();
        
        return posts.Select(x => {
            return ForumAggregator.Domain.PostRegistry.Post.Load(
                x.Id, x.ForumId, x.Title, x.Content, x.Deleted,
                new ForumAggregator.Domain.PostRegistry.PostAuthor(x.AuthorId, false)
            );
        }).ToList();
    }

    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetRecent(int count)
    {
        var posts = _dbContext.Posts
            .Where(x => !x.Deleted)
            .OrderByDescending(x => x.CreatedAt)
            .TakeLast(count)
            .ToList();
        
        return posts.Select(x => {
            return ForumAggregator.Domain.PostRegistry.Post.Load(
                x.Id, x.ForumId, x.Title, x.Content, x.Deleted,
                new ForumAggregator.Domain.PostRegistry.PostAuthor(x.AuthorId, false)
            );
        }).ToList();
    }

    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetAllFromUser(Guid userId)
    {
        var posts = _dbContext.Posts.Where(x => x.AuthorId == userId && !x.Deleted).OrderByDescending(x => x.CreatedAt).ToList();
        
        return posts.Select(x => {
            return ForumAggregator.Domain.PostRegistry.Post.Load(
                x.Id, x.ForumId, x.Title, x.Content, x.Deleted,
                new ForumAggregator.Domain.PostRegistry.PostAuthor(x.AuthorId, false)
            );
        }).ToList();
    }

    public ICollection<ForumAggregator.Domain.PostRegistry.Post> GetAll()
    {
        return  _dbContext.Posts.Where(x => !x.Deleted)
        .OrderByDescending(x => x.CreatedAt)
        .ToList().Select(x => {
            return ForumAggregator.Domain.PostRegistry.Post.Load(
                x.Id, x.ForumId, x.Title, x.Content, x.Deleted,
                new ForumAggregator.Domain.PostRegistry.PostAuthor(x.AuthorId, false)
            );
        }).ToList();
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