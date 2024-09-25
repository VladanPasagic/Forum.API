using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forum.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentService _postService;
    private readonly ILogger<CommentController> _logger;
    private readonly IPolicyCheckService _policyCheckService;

    public CommentController(ICommentService postService, ILogger<CommentController> logger, IPolicyCheckService policyCheckService)
    {
        _postService = postService;
        _logger = logger;
        _policyCheckService = policyCheckService;
    }

    // GET: api/<PostController>
    [HttpGet]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> Get()
    {
        try
        {
            return Ok(await _postService.GetAll());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // GET api/<PostController>/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<CommentResponse>> Get(int id)
    {
        try
        {
            return Ok(await _postService.GetCommentAsync(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // POST api/<PostController>
    [HttpPost]
    [Authorize(Roles = "NoAccessRole")]
    public async Task<IActionResult> Post(CommentRequest request)
    {
        try
        {
            await _postService.Add(request);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // PUT api/<PostController>/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<IActionResult> Put(int id, CommentRequest request)
    {
        try
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return Unauthorized();
            var policy = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("policy", StringComparison.OrdinalIgnoreCase))!.Value;
            if (policy == null || policy.Length == 0)
                return Forbid();
            string userId = user.Value;
            var post = await _postService.GetComment(id);
            if (_policyCheckService.CheckPolicy(post.Room.CategoryId, EF.Entities.RequestType.PUT, policy))
            {
                if (post.UserId != userId)
                {
                    return Forbid();
                }
                await _postService.Update(id, request);
                return Accepted();
            }
            else
            {
                return Forbid();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message + ex.StackTrace, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpPut("{id}/moderator")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<IActionResult> PutModerator(int id, CommentRequest request)
    {
        try
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return Unauthorized();
            await _postService.Update(id, request);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // DELETE api/<PostController>/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return Unauthorized();
            var policy = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("policy", StringComparison.OrdinalIgnoreCase))!.Value;
            if (policy == null || policy.Length == 0)
                return Forbid();
            string userId = user.Value;
            var post = await _postService.GetComment(id);
            if (_policyCheckService.CheckPolicy(post.Room.CategoryId, EF.Entities.RequestType.DELETE, policy))
            {
                if (post.UserId != userId)
                {
                    return Forbid();
                }
                await _postService.DeleteComment(id);
                return Ok();
            }
            else
            {
                return Forbid();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpGet("unposted")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> GetUnposted()
    {
        try
        {
            return Ok(await _postService.GetUnposted());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpPatch("{id}/approve")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> ApproveComment(int id)
    {
        try
        {
            await _postService.ApproveComment(id);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpPatch("{id}/deny")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> DenyComment(int id)
    {
        try
        {
            await _postService.DenyComment(id);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }
}
