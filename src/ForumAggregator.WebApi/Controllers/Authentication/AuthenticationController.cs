namespace ForumAggregator.WebApi.Controllers.Authentication;

using Microsoft.AspNetCore.Mvc;

using ForumAggregator.Application.UseCases;

[ApiController]
[Route("auth")]
public class AuthenticationController: ControllerBase
{
    private readonly IUserRegistrationUseCase _registrationUseCase;

    public AuthenticationController(IUserRegistrationUseCase registrationUseCase)
    {
        _registrationUseCase = registrationUseCase;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest registerRequest)
    {
        // TODO: Create and Save User
        UserUseCaseResult result = _registrationUseCase.Register(
            registerRequest.Name,
            registerRequest.Email,
            registerRequest.Password
        );
        
        if (result.Value == false)
        {
            return UnprocessableEntity(result.Result);
        }

        // TODO: Generate Cookie with Id and UserName Claims

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