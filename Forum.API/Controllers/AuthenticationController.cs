using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forum.API.Controllers;

[Authorize]
[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequests request)
    {
        try
        {
            var result = await _authenticationService.Login(request);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized(result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }


    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequests request)
    {
        try
        {
            var result = await _authenticationService.Register(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [Authorize]
    [HttpPost("2fa")]
    public async Task<IActionResult> HandleTwoFactor(TwoFactorRequests request)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (id != null)
            {
                var result = await _authenticationService.TwoFactorAuthRequest(id.Value, request);
                if (result)
                    return Accepted();
                else
                    return Unauthorized();
            }
            else
            {
                return Unauthorized();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [AllowAnonymous]
    [HttpPatch("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _authenticationService.Logout("");
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }
}
