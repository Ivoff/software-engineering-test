namespace ForumAggregator.WebApi.Controllers.Authentication;

using ForumAggregator.Application.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthenticationController: ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest registerRequest)
    {
        // TODO: Create and Save User

        // TODO: Generate Token with Id and UserName Claims

        // TODO: Return Token, Id and Name as an AuthenticationResponse

        // string token = _authenticationService.GenerateToken(registerRequest.)
        return Ok(registerRequest);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        return Ok(loginRequest);
    }
}