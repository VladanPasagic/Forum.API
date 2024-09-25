using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forum.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }


    // GET: api/<CategoryController>
    [HttpGet]
    [Authorize(Roles = "Admin,Moderator,Member,SuperAdmin,None")]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> Get()
    {
        try
        {
            return Ok(await _categoryService.GetAll());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // GET api/<CategoryController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> Get(int id)
    {
        try
        {
            return Ok(await _categoryService.Get(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // POST api/<CategoryController>
    [HttpPost]
    public async Task<IActionResult> Post(CategoryRequest request)
    {
        try
        {
            await _categoryService.Add(request);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // PUT api/<CategoryController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, CategoryRequest request)
    {
        try
        {
            await _categoryService.Update(id, request);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }

    // DELETE api/<CategoryController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _categoryService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return BadRequest();
        }
    }
}
