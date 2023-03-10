namespace ForumAggregator.WebApi.Controllers.Forum;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using FluentValidation;
using FluentValidation.Results;
using AutoMapper;

using ForumAggregator.Application.UseCases;
using ForumAggregator.Application.Services;

[ApiController]
[Route("/")]
public class ForumController: ControllerBase
{
    private readonly IForumUseCase _forumUseCase;
    private readonly ForumAggregator.Application.Services.IForumService _forumAppService;
    private readonly ForumAggregator.Application.Services.IUserService _userAppService;
    private readonly IValidator<CreateForumRequest> _createForumRequestValidator;
    private readonly IValidator<ModeratorRequest> _moderatorRequestValidator;
    private readonly IValidator<UpdateForumRequest> _updateForumRequestValidator;
    private readonly IValidator<BlackListedRequest> _blackListedRequestValidator;
    private readonly IMapper _mapper;

    public ForumController(
        IForumUseCase ForumUseCase, 
        IValidator<CreateForumRequest> createForumRequestValidator,
        IMapper mapper,
        ForumAggregator.Application.Services.IForumService forumAppService,
        ForumAggregator.Application.Services.IUserService userAppService,
        IValidator<ModeratorRequest> addModeratorRequestValidator,
        IValidator<UpdateForumRequest> updateForumRequestValidator,
        IValidator<BlackListedRequest> blackListedRequestValidator
    )
    {
        _forumUseCase = ForumUseCase;
        _createForumRequestValidator = createForumRequestValidator;
        _mapper = mapper;
        _forumAppService = forumAppService;
        _moderatorRequestValidator = addModeratorRequestValidator;
        _updateForumRequestValidator = updateForumRequestValidator;
        _blackListedRequestValidator = blackListedRequestValidator;
        _userAppService = userAppService;
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

        var result =_forumUseCase.Create(
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

    [HttpPost("forum/moderator")]
    [Authorize]
    public IActionResult AddModerator(ModeratorRequest ModeratorRequest)
    {
        var validationResult = _moderatorRequestValidator.Validate(ModeratorRequest, (options) => {
            options.IncludeRuleSets("Default");
        });

        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var moderators =_mapper.Map<
            ICollection<Controllers.Forum.Moderator>, 
            ICollection<Application.UseCases.ModeratorUseCaseModel>
        >(ModeratorRequest.Moderators);

        var result = _forumUseCase.AddModerator(ModeratorRequest.ForumId, moderators);

        if (result.Value == false)
            return UnprocessableEntity(result.Result);

        var moderatorsId = result.Result.Split(",").Select(x => Guid.Parse(x)).ToList();
        
        return Ok(moderatorsId);
    }

    [HttpPost("forum/blacklisted")]
    [Authorize]
    public IActionResult AddBlackListed(BlackListedRequest addBlackListedRequest)
    {
        var validationResult = _blackListedRequestValidator.Validate(addBlackListedRequest, (options) => {
            options.IncludeRuleSets("Default");
        });

        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ToString());
        
        var result = _forumUseCase.AddBlackListed(
            addBlackListedRequest.forumId,
            _mapper.Map<
                ICollection<Controllers.Forum.BlackListed>, 
                ICollection<Application.UseCases.BlackListedUseCaseModel>
            >(addBlackListedRequest.blackListedUsers)
        );

        if (result.Value == false)
            return UnprocessableEntity(result.Result);
        
        return Ok(result.Result.Split(",").Select(x => Guid.Parse(x)).ToList());
    }

    [HttpPatch("forum")]
    [Authorize]
    public IActionResult UpdateForum(UpdateForumRequest updateRequest)
    {
        var validationResult = _updateForumRequestValidator.Validate(updateRequest);
        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }
        
        var result = _forumAppService.UpdateForum(updateRequest.forumId, updateRequest.Name, updateRequest.Description);

        if (result.Value == false)
            return UnprocessableEntity(result.Result);
        
        return Ok();
    }

    [HttpPatch("forum/moderator")]
    [Authorize]
    public IActionResult UpdateModeratorAuthorities(ModeratorRequest updateModeratorRequest)
    {
        var validationResult = _moderatorRequestValidator.Validate(updateModeratorRequest, (options) => {
            options.IncludeRuleSets("Default");
        });

        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var result = _forumUseCase.UpdateModeratorAuthorities(
            updateModeratorRequest.ForumId,
            _mapper.Map<ICollection<Moderator>, ICollection<ModeratorUseCaseModel>>(updateModeratorRequest.Moderators)
        );

        if (result.Value == false)
        {
            return UnprocessableEntity(result.Result);
        }

        return Ok(result.Result.Split(",").Select(x => Guid.Parse(x)).ToList());
    }

    [HttpPatch("forum/blacklisted")]
    [Authorize]
    public IActionResult UpdateBlackListed(BlackListedRequest updateBlackListedRequest)
    {
        var validationResult = _blackListedRequestValidator.Validate(updateBlackListedRequest, (options) => {
            options.IncludeRuleSets("Default");
        });

        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ToString());

        var result = _forumUseCase.UpdateBlackListed(
            updateBlackListedRequest.forumId, 
            _mapper.Map<
                ICollection<Controllers.Forum.BlackListed>, 
                ICollection<Application.UseCases.BlackListedUseCaseModel>
            >(updateBlackListedRequest.blackListedUsers)
        );

        if (result.Value == false)
            return UnprocessableEntity(result.Result);
        
        return Ok(result.Result.Split(",").Select(x => Guid.Parse(x)).ToList());
    }

    [HttpDelete("forum/{forumId}")]
    [Authorize]
    public IActionResult DeleteForum(Guid forumId)
    {
        if (_forumAppService.GetForum(forumId) == null)
            return NotFound(forumId);

        var result = _forumUseCase.Delete(forumId);
        if (result.Value == false)
            return UnprocessableEntity(result.Result);
        
        return Ok();
    }

    [HttpDelete("forum/moderator")]
    [Authorize]
    public IActionResult DeleteModerator(ModeratorRequest deleteModeratorRequest)
    {
        var validationResult = _moderatorRequestValidator.Validate(deleteModeratorRequest, (options) => {
            options.IncludeRuleSets("Delete");
        });

        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var result = _forumUseCase.RemoveModerator(
            deleteModeratorRequest.ForumId, 
            _mapper.Map<ICollection<Moderator>, ICollection<ModeratorUseCaseModel>>(deleteModeratorRequest.Moderators)
        );
        
        if (result.Value == false)
        {
            return UnprocessableEntity(result.Result);
        }

        return Ok(result.Result.Split(",").Select(x => Guid.Parse(x)).ToList());
    }

    [HttpDelete("forum/blacklisted")]
    [Authorize]
    public IActionResult DeleteBlackListed(BlackListedRequest deleteBlackListedRequest)
    {
        var validationResult = _blackListedRequestValidator.Validate(deleteBlackListedRequest, (options) => {
            options.IncludeRuleSets("Delete");
        });

        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ToString());
        
        var result = _forumUseCase.RemoveBlackListed(
            deleteBlackListedRequest.forumId,
            _mapper.Map<
                ICollection<Controllers.Forum.BlackListed>, 
                ICollection<Application.UseCases.BlackListedUseCaseModel>
            >(deleteBlackListedRequest.blackListedUsers)
        );

        if (result.Value == false)
            return UnprocessableEntity(result.Result);
        
        return Ok(result.Result.Split(",").Select(x => Guid.Parse(x)).ToList());
    }

    [HttpGet("forum/{forumId}")]
    [AllowAnonymous]
    public IActionResult ReadForum(string forumId)
    {
        ForumAppServiceModel? forum;
        Guid guidForumId;
        
        if (Guid.TryParse(forumId, out guidForumId) == true)
            forum = _forumAppService.GetForum(guidForumId);
        else
            forum = _forumAppService.GetForumByName(forumId);


        if (forum == null)
            return NotFound($"Forum {forumId} not found.");
        
        return Ok(_mapper.Map<ReadForumResponse>(forum));
    }

    [HttpGet("forum/{forumId}/moderator/{moderatorId}")]
    [AllowAnonymous]
    public IActionResult ReadModerator(string forumId, string moderatorId)
    {
        ForumAppServiceModel? forum;
        ModeratorAppServiceModel? moderator;
        Guid guidForumId;
        Guid guidModeratorId;
        
        if (Guid.TryParse(forumId, out guidForumId) == true && Guid.TryParse(moderatorId, out guidModeratorId) == true)
        {
            moderator = _forumUseCase.GetModerator(guidForumId, guidModeratorId);
            if (moderator == null)
                moderator = _forumUseCase.GetModeratorByUserId(guidForumId, guidModeratorId);
        }
        else
        {
            if (Guid.TryParse(moderatorId, out guidModeratorId) == false)
                return NotFound($"Moderator {moderatorId} not found.");
            
            forum = _forumAppService.GetForumByName(forumId);
            if (forum == null)
                return NotFound($"Forum {forumId} not found.");

            moderator = _forumUseCase.GetModerator(forum.Id, guidModeratorId);
            if (moderator == null)
                moderator = _forumUseCase.GetModeratorByUserId(guidForumId, guidModeratorId);
        }

        if (moderator == null)
            return NotFound($"Moderator {moderatorId} not found.");

        return Ok(new ReadModeratorResponse(
            moderator.Id,
            guidForumId,
            moderator.UserId,
            moderator.Deleted,
            moderator.Authorities.Select(x => new AuthorityResponse(x.ToString(), (int) x)).ToList()
        ));
    }

    [HttpGet("forum/{forumId}/blacklisted/{userId}")]
    [AllowAnonymous]
    public IActionResult ReadBlackListed(string forumId, string userId)
    {
        ForumAppServiceModel? forum;
        BlackListedAppServiceModel? blackListed;
        Guid guidForumId;
        Guid guidUserId;
        
        if (Guid.TryParse(forumId, out guidForumId) == true && Guid.TryParse(userId, out guidUserId) == true)
        {
            blackListed = _forumUseCase.GetBlackListed(guidForumId, guidUserId);
            if (blackListed == null)
                blackListed = _forumUseCase.GetBlackListedByUserId(guidForumId, guidUserId);
        }
        else
        {
            if (Guid.TryParse(userId, out guidUserId) == false)
                return NotFound($"BlackListed {guidUserId} not found.");
            
            forum = _forumAppService.GetForumByName(forumId);
            if (forum == null)
                return NotFound($"Forum {forumId} not found.");

            blackListed = _forumUseCase.GetBlackListed(forum.Id, guidUserId);
            if (blackListed == null)
                blackListed = _forumUseCase.GetBlackListedByUserId(guidForumId, guidUserId);
        }

        if (blackListed == null)
            return NotFound($"BlackListed {guidUserId} not found.");

        return Ok(new ReadBlackListedResponse(
            blackListed.Id,
            guidForumId,
            blackListed.UserId,
            blackListed.CanComment,
            blackListed.CanPost,
            blackListed.Deleted
        ));
    }

    [HttpGet("forum/{forumId}/moderator")]
    [AllowAnonymous]
    public IActionResult ReadAllModerators(string forumId)
    {
        Guid guidForumId;
        ICollection<ModeratorAppServiceModel> moderators;
        
        if (Guid.TryParse(forumId, out guidForumId) == true)
        {
            moderators = _forumUseCase.GetAllModerators(guidForumId);
        }
        else
        {
            var forum = _forumAppService.GetForumByName(forumId);
            if (forum == null)
                return NotFound($"Forum {forumId} not found.");

            moderators = _forumUseCase.GetAllModerators(forum.Id);
            guidForumId = forum.Id;
        }

        return Ok(
            moderators.Select(x => new ReadModeratorResponse(
                x.Id, 
                guidForumId, 
                x.UserId, 
                x.Deleted,
                x.Authorities.Select(y => new AuthorityResponse(y.ToString(), (int) y)).ToList()
            )).ToList()
        );
    }

    [HttpGet("forum/{forumId}/blacklisted")]
    [AllowAnonymous]
    public IActionResult ReadAllBlackListed(string forumId)
    {
        Guid guidForumId;
        ICollection<BlackListedAppServiceModel> blackList;

        if (Guid.TryParse(forumId, out guidForumId) == true)
        {
            blackList = _forumUseCase.GetAllBlackListed(guidForumId);
        }
        else
        {
            var forum = _forumAppService.GetForumByName(forumId);
            if (forum == null)
                return NotFound($"Forum {forumId} not found.");
            
            blackList = _forumUseCase.GetAllBlackListed(forum.Id);
            guidForumId = forum.Id;
        }

        return Ok(
            blackList.Select(x => new ReadBlackListedResponse(
                x.Id, guidForumId, x.UserId, x.CanComment, x.CanPost, x.Deleted
            )).ToList()
        );
    }

    [HttpGet("forum")]
    [AllowAnonymous]
    public IActionResult ReadAllForums()
    {
        return Ok(_forumAppService.GetAllForums().Select(x => _mapper.Map<ReadForumResponse>(x)).ToList());
    }

    [HttpGet("forum/user/{userId}")]
    [AllowAnonymous]
    public IActionResult ReadAllForumsFromUser(string userId)
    {
        Guid guidUserId;
        if (Guid.TryParse(userId, out guidUserId))
        {
            return Ok(_forumAppService.GetAllForumsFromUser(guidUserId).Select(x => _mapper.Map<ReadForumResponse>(x)).ToList());
        }
        else
        {
            var user = _userAppService.GetUserByName(userId);
            if (user == null)
                return NotFound(userId);
            
            return Ok(_forumAppService.GetAllForumsFromUser(user.Id).Select(x => _mapper.Map<ReadForumResponse>(x)).ToList());
        }
    }

    [HttpGet("forum/search/{searchString}")]
    [Authorize]
    public IActionResult SearchForums(string searchString)
    {
        return Ok(_forumAppService.SearchForums(searchString).Select(x => _mapper.Map<ReadForumResponse>(x)).ToList());
    }
}