using BlogApp.Models;
using BlogApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_blogService.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var post = _blogService.GetById(id);
        return post == null ? NotFound() : Ok(post);
    }

    [HttpPost]
    public IActionResult Create(BlogPost post)
    {
        var createdPost = _blogService.Create(post);
        return CreatedAtAction(nameof(GetById), new { id = createdPost.Id }, createdPost);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, BlogPost updatedPost)
    {
        if (_blogService.Update(id, updatedPost))
            return NoContent();
        return NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (_blogService.Delete(id))
            return NoContent();
        return NotFound();
    }
}
