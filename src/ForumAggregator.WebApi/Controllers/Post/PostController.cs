namespace ForumAggregator.WebApi.Controllers.Post;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using FluentValidation;

using ForumAggregator.Application.UseCases;
using ForumAggregator.Application;

[ApiController]
[Route("/")]
public class PostController: ControllerBase
{
    private readonly IPostUseCase _postUseCase;
    private readonly IValidator<AddPostRequest> _addPostRequestValidator;
    private readonly IValidator<UpdatePostRequest> _updatePostRequestValidator;
    private readonly IAppContext _appContext;
    private readonly ForumAggregator.Application.Services.IPostService _postService;

    public PostController(
        IPostUseCase postUseCase,
        IValidator<AddPostRequest> addPostRequestValidator,
        IValidator<UpdatePostRequest> updatePostRequestValidator,
        IAppContext appContext,
        ForumAggregator.Application.Services.IPostService postService
    )
    {
        _postUseCase = postUseCase;
        _addPostRequestValidator = addPostRequestValidator;
        _appContext = appContext;
        _updatePostRequestValidator = updatePostRequestValidator;
        _postService = postService;
    }

    [HttpPost("post")]
    [Authorize]
    public IActionResult AddPost(AddPostRequest addPostRequest)
    {
        var validationResult = _addPostRequestValidator.Validate(addPostRequest);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ToString());
        
        var result = _postUseCase.Create(
            addPostRequest.ForumId, 
            _appContext.UserId, 
            addPostRequest.Title, 
            addPostRequest.Content
        );

        if (result.Value == false || result.EntityUseCaseDto == null)
            return UnprocessableEntity(result.Result);
        
        return Ok(result.EntityUseCaseDto);
    }

    [HttpPatch("post")]
    [Authorize]
    public IActionResult UpdatePost(UpdatePostRequest updatePostRequest)
    {
        var validationResult = _updatePostRequestValidator.Validate(updatePostRequest);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ToString());
        
        var result = _postService.UpdatePost(
            updatePostRequest.PostId,
            updatePostRequest.Title,
            updatePostRequest.Content
        );

        if (result.Value == false)
            return UnprocessableEntity(result.Result);
        
        return Ok();
    }
}