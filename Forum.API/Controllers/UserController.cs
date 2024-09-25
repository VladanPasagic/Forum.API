using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forum.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    // GET: api/<UserController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> Get()
    {
        try
        {
            return Ok(await _userService.GetUsers());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SingleUserResponse>> Get(string id)
    {
        try
        {
            return Ok(await _userService.GetSingle(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // POST api/<UserController>
    [HttpPost]
    [Authorize(Roles = "NoAccessRole")]
    public async Task<IActionResult> Post(UserRequest request)
    {
        try
        {
            //await _userService.Add(request);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    [Authorize(Roles ="Admin,SuperAdmin")]
    public async Task<IActionResult> Put(string id, SingleUserRequest request)
    {
        try
        {
            foreach(var perm in request.Permissions)
            {
                Console.WriteLine(perm.Id);
            }
            await _userService.UpdateUserPermissions(id, request);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // DELETE api/<UserController>/5
    [HttpDelete("{id}")]
    [Authorize(Roles ="NoAccessRole")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _userService.Delete(id);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpPatch("{id}/approve")]
    [Authorize(Roles ="Admin,SuperAdmin")]
    public async Task<IActionResult> ApproveUser(string id)
    {
        try
        {
            await _userService.ApproveUser(id);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpPatch("{id}/deny")]
    [Authorize(Roles ="Admin,SuperAdmin")]
    public async Task<IActionResult> DenyUser(string id)
    {
        try
        {
            await _userService.DenyUser(id);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpGet("requests")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetRequests()
    {
        try
        {
            return Ok(await _userService.GetRequests());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

}
