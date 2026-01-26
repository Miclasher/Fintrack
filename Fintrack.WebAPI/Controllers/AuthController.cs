using Fintrack.Services.Abstractions;
using Fintrack.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] UserLoginDTO request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }

    [HttpPost("loginWithRefreshToken")]
    public async Task<ActionResult<AuthResponseDTO>> LoginWithRefreshToken([FromBody] string refreshToken)
    {
        var response = await _authService.RefreshTokenAsync(refreshToken);

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] UserRegisterDTO request)
    {
        var response = await _authService.RegisterAsync(request);
        return Ok(response);
    }
}
