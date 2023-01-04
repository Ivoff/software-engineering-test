namespace ForumAggregator.WebApi.Controllers.Authentication;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ForumAggregator.Application.UseCases;

[ApiController]
[Route("auth")]
public class AuthenticationController: ControllerBase
{
    private readonly IUserRegistrationUseCase _registrationUseCase;
    private readonly Application.Services.IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        IUserRegistrationUseCase registrationUseCase, 
        Application.Services.IAuthenticationService 
        authenticationService, 
        ILogger<AuthenticationController> logger
    )
    {
        _registrationUseCase = registrationUseCase;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register(RegisterRequest registerRequest)
    {
        _logger.LogInformation("Register Controller");

        UserUseCaseResult result = _registrationUseCase.Register(
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

        AuthenticationResponse response = new AuthenticationResponse(registrationResult.Id, registrationResult.Name);

        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login(LoginRequest loginRequest)
    {
        return Ok(loginRequest);
    }

    [HttpGet("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        _authenticationService.SignOut();
        return Ok();
    }
}