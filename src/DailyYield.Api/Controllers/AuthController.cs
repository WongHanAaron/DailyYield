using DailyYield.Api.Controllers;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using DailyYield.Infrastructure.Adapters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyYield.Api.Controllers;

[AllowAnonymous]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IRepository<User> _userRepository;

    public AuthController(IAuthService authService, IRepository<User> userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Check if user exists
        var existingUsers = await _userRepository.GetAllAsync();
        if (existingUsers.Any(u => u.Email == request.Email))
        {
            return BadRequest("User already exists");
        }

        var user = new User
        {
            Email = request.Email,
            PasswordHash = ((AuthService)_authService).HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        await _userRepository.AddAsync(user);

        var token = await _authService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.ValidateUser(request.Email, request.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        var token = await _authService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}