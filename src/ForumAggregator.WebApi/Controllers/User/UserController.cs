namespace ForumAggregator.WebApi.Controllers.User;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;

using ForumAggregator.Application.Services;

[ApiController]
[Route("/")]
public class UserController: ControllerBase
{
    private readonly IValidator<UpdateUserRequest> _updateValidator;
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IValidator<UpdateUserRequest> updateValidator, 
        IUserService userService,
        ILogger<UserController> logger
    )
    {
        _updateValidator = updateValidator;
        _userService = userService;
        _logger = logger;
    }

    [HttpPatch("user")]
    [Authorize]
    public IActionResult UpdateUser(UpdateUserRequest updateRequest)
    {
        var validationResult = _updateValidator.Validate(updateRequest, (options) => {
            if (!string.IsNullOrWhiteSpace(updateRequest.NewEmail))
                options.IncludeRuleSets("Email");
            
            if (!string.IsNullOrWhiteSpace(updateRequest.NewName))
                options.IncludeRuleSets("Name");
            
            if (!string.IsNullOrWhiteSpace(updateRequest.NewPassword))
                options.IncludeRuleSets("Password");
        });

        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var updateResult = _userService.UpdateUser(new UserAppServiceModel() {
            Id = updateRequest.Id,
            Name = updateRequest.NewName,
            Email = updateRequest.NewEmail,
            Password = updateRequest.NewPassword
        });

        if (updateResult.Value == false)
        {
            return UnprocessableEntity(updateResult.Result);
        }

        return Ok();
    }

    [HttpDelete("user")]
    [Authorize]
    public IActionResult DeleteUser()
    {
        throw new NotImplementedException();
    }

    [HttpGet("user/{userId}")]
    [AllowAnonymous]
    public IActionResult ReadUser(string userId)
    {
        UserAppServiceModel? user;
        Guid userGuid = Guid.Empty;

        try
        {
            userGuid = Guid.Parse(userId);
        }
        catch(Exception e)
        {
            string errorMessage = e.Message;
            user = _userService.GetUserByName(userId);
            
            if (user == null)
            {
                return NotFound(string.Join("\n", new[]{errorMessage, $"User {userId} not found."}));
            }

            return Ok(new ReadUserReponse(
                user.Id,
                user.Name,
                user.Email,
                user.Deleted
            ));
        }

        user = _userService.GetUser(userGuid);
        if (user == null)
        {
            return NotFound($"User {userId} not found.");
        }

        return Ok(new ReadUserReponse(
            user.Id,
            user.Name,
            user.Email,
            user.Deleted
        ));
    }

    [HttpGet("user")]
    [AllowAnonymous]
    public IActionResult ReadAllUsers()
    {
        return Ok(
            _userService.GetAll().Select(
                user => new ReadUserReponse(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Deleted
            ))
        );
    }
}