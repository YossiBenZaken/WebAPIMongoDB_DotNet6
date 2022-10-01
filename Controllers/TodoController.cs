using TodoApi.Models;
using TodoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class TodoController : ControllerBase
{
    private readonly TodoService _todoService;
    public TodoController(TodoService todoService)
    {
        _todoService = todoService;
    }
    [HttpGet]
    public async Task<IEnumerable<TodoItem>> Get() => await _todoService.GetAsync();
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> Get(string id)
    {
        var todoItem = await _todoService.GetAsync(id);
        if (todoItem is null)
        {
            return NotFound();
        }
        return todoItem;
    }
    /// <summary>
    /// Creates a TodoItem.
    /// </summary>
    /// <param name="todoItem"></param>
    /// <returns>A newly created TodoItem</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Todo
    ///     {
    ///        "id": 1,
    ///        "name": "Item #1",
    ///        "isComplete": true
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(TodoItem todoItem)
    {
        await _todoService.CreateAsync(todoItem);
        return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, TodoItem _todoItem)
    {
        var todoItem = await _todoService.GetAsync(id);
        if (todoItem is null)
        {
            return NotFound();
        }
        _todoItem.Id = todoItem.Id;
        await _todoService.UpdateAsync(id, _todoItem);

        return NoContent();
    }
    /// <summary>
    /// Deletes a specific TodoItem
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<TodoItem>> Delete(string id)
    {
        var todoItem = await _todoService.GetAsync(id);
        if (todoItem is null)
        {
            return NotFound();
        }
        await _todoService.RemoveAsync(id);
        return NoContent();
    }

}