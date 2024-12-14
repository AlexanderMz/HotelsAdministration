using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelsAdministration.Domain.Models.Auth;
using HotelsAdministration.Application.Interfaces;

namespace HotelsAdministration.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Authentication")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticate a travel agent
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token for authentication</returns>
    /// <response code="200">Returns the JWT token</response>
    /// <response code="401">Invalid credentials</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
    {
        try
        {
            var agent = await _authService.Authenticate(request.Email, request.Password);

            if (agent == null)
                return Unauthorized("Invalid email or password");

            var token = await _authService.GenerateJwtToken(agent);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in login");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Register a new travel agent
    /// </summary>
    /// <param name="request">New agent data</param>
    /// <returns>JWT token for authentication</returns>
    /// <response code="200">Successful registration</response>
    /// <response code="400">Invalid data or email already exists</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponse>> Register(RegisterRequest request)
    {
        try
        {
            var agent = await _authService.Register(request);
            var token = await _authService.GenerateJwtToken(agent);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in registration");
            return StatusCode(500, "Internal server error");
        }
    }
}