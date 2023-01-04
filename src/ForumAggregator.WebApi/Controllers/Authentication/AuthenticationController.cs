namespace ForumAggregator.WebApi.Controllers.Authentication;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;

using ForumAggregator.Application.UseCases;
using ForumAggregator.Application.Services;

[ApiController]
[Route("auth")]
public class AuthenticationController: ControllerBase
{
    private readonly IUserAuthenticationUseCase _userAuthenticationUseCase;    
    private readonly Application.Services.IAuthenticationService _authenticationService;
    private readonly IUserService _user_service;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IValidator<RegisterRequest> _registerRequestValidator;
    private readonly IValidator<LoginRequest> _loginRequestValidator;

    public AuthenticationController(
        IUserAuthenticationUseCase userAuthenticationUseCase, 
        Application.Services.IAuthenticationService authenticationService, 
        IUserService user_service,
        IValidator<RegisterRequest> registerRequestValidator,
        IValidator<LoginRequest> loginRequestValidator,
        ILogger<AuthenticationController> logger
    )
    {
        _userAuthenticationUseCase = userAuthenticationUseCase;
        _authenticationService = authenticationService;
        _user_service = user_service;
        _loginRequestValidator = loginRequestValidator;
        _registerRequestValidator = registerRequestValidator;
        _logger = logger;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register(RegisterRequest registerRequest)
    {
        _logger.LogInformation("Register Controller");

        ValidationResult validationResult = _registerRequestValidator.Validate(registerRequest);
        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        UserUseCaseResult result = _userAuthenticationUseCase.Register(
            registerRequest.Name,
            registerRequest.Email,
            registerRequest.Password
        );
        
        if (result.Value == false || result.UseCaseModel == null)
        {
            return UnprocessableEntity(result.Result);
        }

        UserUseCaseModel registrationResult = (UserUseCaseModel) result.UseCaseModel;
        _authenticationService.GenerateCookie(registrationResult.Id, registrationResult.Name);

        return Ok(new AuthenticationResponse(registrationResult.Id, registrationResult.Name));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login(LoginRequest loginRequest)
    {
        _logger.LogInformation("Login Controller");

        ValidationResult validationResult = _loginRequestValidator.Validate(loginRequest);
        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        if (HttpContext.User.Identity?.IsAuthenticated == true)
        {
            _logger.LogInformation("Already Logged in");

            UserAppServiceModel user = _user_service.GetUser(loginRequest.Email) ?? new UserAppServiceModel();
            return Ok(new AuthenticationResponse(user.Id, user.Name));
        }

        UserUseCaseResult result = _userAuthenticationUseCase.Login(
            loginRequest.Email,
            loginRequest.Password
        );

        if (result.Value == false || result.UseCaseModel == null)
        {
            return Unauthorized(result.Result);
        }

        UserUseCaseModel loginResult = (UserUseCaseModel) result.UseCaseModel;
        _authenticationService.GenerateCookie(loginResult.Id, loginResult.Name);

        return Ok(new AuthenticationResponse(loginResult.Id, loginResult.Name));
    }

    [HttpGet("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        string? userIdStr =  HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr == null)
        {
            return Unauthorized("Some Claim or Claims are not set.");
        }

        Guid userId = Guid.Parse(userIdStr);

        _logger.LogInformation($"Logout Controller\nUser: {userId}");
        _authenticationService.SignOut();
        return Ok();
    }
}