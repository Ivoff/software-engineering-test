namespace ForumAggregator.Application.Services;

using AutoMapper;
using System;

using ForumAggregator.Domain.PostRegistry;
using ForumAggregator.Domain.Shared.Interfaces;
using System.Collections.Generic;

public class PostService : IPostService
{
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly IForumRepository _forumRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAppContext _appContext;

    public PostService
    (
        IMapper mapper, 
        IPostRepository postRepository, 
        IAppContext appContext,
        IForumRepository forumRepository,
        IUserRepository userRepository
    )
    {
        _mapper = mapper;
        _postRepository = postRepository;
        _appContext = appContext;
        _forumRepository = forumRepository;
        _userRepository = userRepository;
    }

    public PostAppServiceModel? GetPost(Guid postId)
    {
        var post = _postRepository.Get(postId);
        if(post == null)
            return null;
        
        var appPost = _mapper.Map<PostAppServiceModel>(post);
        appPost.AuthorName = _userRepository.Get(appPost.AuthorId)?.Name ?? string.Empty;
        appPost.ForumName = _forumRepository.Get(appPost.ForumId)?.Name ?? string.Empty;

        return appPost;
    }

    public ServiceResult DeletePost(Guid postId)
    {
        var post = _postRepository.Get(postId);
        if(post == null)
            return new ServiceResult(false, $"Post {postId} does not exist.");
        
        var resultRemove = post.Remove(_appContext.UserId);

        if (resultRemove.Value == false)
            return new ServiceResult(false, resultRemove.Result);

        var result = _postRepository.Save(post);
        
        return new ServiceResult(
            result, 
            result ? string.Empty : "Something wrong happened during data persistance"
        );
    }

    public ServiceResult UpdatePost(Guid postId, string newTitle, string newContent)
    {
        var domainPost = _postRepository.Get(postId);
        if (domainPost == null)
            return new ServiceResult(false, $"Post {postId} does not exist.");
        
        domainPost.Update(
            _appContext.UserId,
            string.IsNullOrEmpty(newTitle) ? domainPost.Title : newTitle,
            string.IsNullOrEmpty(newContent) ? domainPost.Content : newContent
        );

        var result = _postRepository.Save(domainPost);
        return new ServiceResult(
            result,
            result ? string.Empty : "Something wrong happened during data persistance"
        );
    }

    public ICollection<PostAppServiceModel> GetAllPosts()
    {
        var posts = _postRepository.GetAll();
        var appPosts = _mapper.Map<ICollection<Post>, ICollection<PostAppServiceModel>>(posts);
        
        return appPosts.Select(x => {
            x.AuthorName = _userRepository.Get(x.AuthorId)?.Name ?? string.Empty;
            x.ForumName = _forumRepository.Get(x.ForumId)?.Name ?? string.Empty;
            return x;
        }).ToList();
    }

    public ICollection<PostAppServiceModel> GetAllPostsFromUser(Guid userId)
    {
        var posts = _postRepository.GetAllFromUser(userId);
        var appPosts = _mapper.Map<ICollection<Post>, ICollection<PostAppServiceModel>>(posts);

        return appPosts.Select(x => {
            x.AuthorName = _userRepository.Get(x.AuthorId)?.Name ?? string.Empty;
            x.ForumName = _forumRepository.Get(x.ForumId)?.Name ?? string.Empty;
            return x;
        }).ToList();
    }

    public ICollection<PostAppServiceModel> GetAllPostsFromForum(Guid forumId)
    {
        var posts = _postRepository.GetAllFromForum(forumId);
        var appPosts = _mapper.Map<ICollection<Post>, ICollection<PostAppServiceModel>>(posts);
        
        return appPosts.Select(x => {
            x.AuthorName = _userRepository.Get(x.AuthorId)?.Name ?? string.Empty;
            x.ForumName = _forumRepository.Get(x.ForumId)?.Name ?? string.Empty;
            return x;
        }).ToList();
    }

    public ICollection<PostAppServiceModel> GetRecentPosts(int count)
    {
        var posts = _postRepository.GetRecent(count);
        var appPosts = _mapper.Map<ICollection<Post>, ICollection<PostAppServiceModel>>(posts);

        return appPosts.Select(x => {
            x.AuthorName = _userRepository.Get(x.AuthorId)?.Name ?? string.Empty;
            x.ForumName = _forumRepository.Get(x.ForumId)?.Name ?? string.Empty;
            return x;
        }).ToList();
    }
}