using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forum.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoomController : ControllerBase
{
    private readonly ILogger<RoomController> _logger;
    private readonly IRoomService _roomService;
    private readonly ICategoryService _categoryService;
    private readonly ICommentService _commentService;
    private readonly IPolicyCheckService _policyCheckService;

    public RoomController(ILogger<RoomController> logger, IRoomService roomService, ICategoryService categoryService, ICommentService commentService, IPolicyCheckService policyCheckService)
    {
        _logger = logger;
        _roomService = roomService;
        _categoryService = categoryService;
        _commentService = commentService;
        _policyCheckService = policyCheckService;
    }

    // GET: api/<ChannelController>
    [HttpGet]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<IEnumerable<RoomResponse>>> Get()
    {
        try
        {
            return Ok(await _roomService.GetAllOpenedRooms());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpGet("category/{id}")]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<RoomResponse>> GetByCategoryId(int id)
    {
        try
        {
            return Ok(await _roomService.GetAllByCategoryId(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }


    // GET api/<ChannelController>/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<RoomResponse>> Get(int id)
    {
        try
        {
            return Ok(await _roomService.GetSingleRoom(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpGet("{id}/moderator")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<RoomResponse>> GetModerator(int id)
    {
        try
        {
            return Ok(await _roomService.Get(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpGet("{id}/posts")]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> GetAllPosts(int id)
    {
        try
        {
            return Ok(await _roomService.GetAllComments(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // POST api/<ChannelController>
    [HttpPost]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<IActionResult> Post(RoomRequest request)
    {
        try
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (id == null)
                return Unauthorized();
            string userId = id.Value;
            var policy = HttpContext.User.Claims.Where(c => c.Type.Equals("policy", StringComparison.OrdinalIgnoreCase))!.First().Value;
            if (policy == null || policy.Length == 0)
                return Forbid();
            if (_policyCheckService.CheckPolicy(request.CategoryId, RequestType.POST, policy))
            {
                await _roomService.CreateNew(userId, request);
                return Accepted();
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

    // PUT api/<ChannelController>/5
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, RoomUpdateRequest request)
    {
        try
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return Unauthorized();
            string userId = user.Value;
            var policy = HttpContext.User.Claims.Where(c => c.Type.Equals("policy", StringComparison.OrdinalIgnoreCase))!.First().Value;
            if (policy == null || policy.Length == 0)
                return Forbid();
            var room = await _roomService.GetSingleRoom(id);
            if (_policyCheckService.CheckPolicy(request.CategoryId, RequestType.PUT, policy))
            {
                if (userId != room.UserId)
                {
                    return Forbid();
                }
                await _roomService.UpdateRoom(id, request);
                return Accepted();
            }
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpPut("{id}/moderator")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<IActionResult> PutModerator(int id, RoomUpdateRequest request)
    {
        try
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return Unauthorized();
            await _roomService.UpdateModerator(id, request);
            return Accepted();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // DELETE api/<ChannelController>/5
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return Unauthorized();
            string userId = user.Value;
            var policy = HttpContext.User.Claims.Where(c => c.Type.Equals("policy", StringComparison.OrdinalIgnoreCase))!.First().Value;
            if (policy == null || policy.Length == 0)
                return Forbid();
            var channel = await _roomService.Get(id);
            if (_policyCheckService.CheckPolicy(channel!.CategoryId, RequestType.DELETE, policy))
            {
                if (userId != channel.UserId)
                    return Forbid();
                await _roomService.DeleteRoom(id);
                return Accepted();
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
   

    [HttpPost("{id}/post")]
    [Authorize(Roles = "Member,Moderator,Admin,SuperAdmin")]
    public async Task<IActionResult> PostInChannel(int id, CommentInRoomRequest request)
    {
        try
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return Unauthorized();
            string userId = user.Value;
            var policy = HttpContext.User.Claims.Where(c => c.Type.Equals("policy", StringComparison.OrdinalIgnoreCase))!.First().Value;
            if (policy == null || policy.Length == 0)
                return Forbid();
            var channel = await _roomService.Get(id);
            if (_policyCheckService.CheckPolicy(channel!.CategoryId, RequestType.POST, policy))
            {
                await _commentService.AddPost(userId, request);
                return Accepted();
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

    [HttpGet("unopened")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<ActionResult<IEnumerable<RoomResponse>>> GetUnopened()
    {
        try
        {
            return Ok(await _roomService.GetUnopened());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    [HttpPatch("{id}/approve")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<IActionResult> ApproveRoom(int id)
    {
        try
        {
            await _roomService.ApproveRoom(id);
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
    public async Task<IActionResult> DenyRoom(int id)
    {
        try
        {
            await _roomService.DenyRoom(id);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }
}
