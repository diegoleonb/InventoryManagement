using InventoryAPI.DTOs;
using InventoryAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
  /// <summary>
  /// Controller for user authentication (registration and login).
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDto>> Register(UserRegisterDto registerDto)
    {
      try
      {
        var result = await _authService.Register(registerDto);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Logins an existing user. Verifies credentials and returns a JWT token.
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<UserResponseDto>> Login(UserLoginDto loginDto)
    {
      try
      {
        var result = await _authService.Login(loginDto);
        return Ok(result);
      }
      catch (UnauthorizedAccessException)
      {
        return Unauthorized("Invalid credentials");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}