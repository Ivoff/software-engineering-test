namespace ForumAggregator.WebApi.Controllers.Forum;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using FluentValidation;
using FluentValidation.Results;
using AutoMapper;

using ForumAggregator.Application.UseCases;

[ApiController]
[Route("/")]
public class ForumController: ControllerBase
{
    private readonly IForumCreationUseCase _forumCreationUseCase;
    private readonly IValidator<CreateForumRequest> _createForumRequestValidator;
    private readonly IMapper _mapper;

    public ForumController(
        IForumCreationUseCase forumCreationUseCase, 
        IValidator<CreateForumRequest> createForumRequestValidator,
        IMapper mapper
    )
    {
        _forumCreationUseCase = forumCreationUseCase;
        _createForumRequestValidator = createForumRequestValidator;
        _mapper = mapper;
    }

    [HttpPost("forum")]
    [Authorize]
    public IActionResult CreateForum(CreateForumRequest createForumRequest)
    {
        var validationResult = _createForumRequestValidator.Validate(createForumRequest);
        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var result =_forumCreationUseCase.Create(
            createForumRequest.Name,
            createForumRequest.Description,
            _mapper.Map<
                ICollection<Controllers.Forum.Moderator>, 
                ICollection<Application.UseCases.ModeratorUseCaseModel>
            >(createForumRequest.Moderators),
            _mapper.Map<
                ICollection<Controllers.Forum.BlackListed>, 
                ICollection<Application.UseCases.BlackListedUseCaseModel>
            >(createForumRequest.BlackList)
        );

        if (result.Value == false || result.EntityUseCaseDto == null)
        {
            return UnprocessableEntity(result.Result);
        }

        return Ok(new CreateForumResponse(
            result.EntityUseCaseDto.Id, 
            result.EntityUseCaseDto.Name
        ));

        throw new NotImplementedException();
    }

    [HttpPatch("forum")]
    [Authorize]
    public IActionResult UpdateForum()
    {
        throw new NotImplementedException();
    }

    [HttpDelete("forum")]
    [Authorize]
    public IActionResult DeleteForum()
    {
        throw new NotImplementedException();
    }

    [HttpGet("forum/{forumId}")]
    [AllowAnonymous]
    public IActionResult ReadForum(string forumId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("forum")]
    [AllowAnonymous]
    public IActionResult ReadAllForums()
    {
        throw new NotImplementedException();
    }
}