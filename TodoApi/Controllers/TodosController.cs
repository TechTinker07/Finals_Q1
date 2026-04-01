using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private static readonly List<Todo> _todos = new();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_todos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Todo todo)
        {
            if (string.IsNullOrWhiteSpace(todo.Title))
            {
                return BadRequest("Title is required.");
            }

            todo.Id = Guid.NewGuid();

            if (todo.CreatedAt == default)
            {
                todo.CreatedAt = DateTime.UtcNow;
            }

            _todos.Add(todo);

            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }


        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Todo updatedTodo)
        {
            var existingTodo = _todos.FirstOrDefault(t => t.Id == id);

            if (existingTodo == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(updatedTodo.Title))
            {
                return BadRequest("Title is required.");
            }

            existingTodo.Title = updatedTodo.Title;
            existingTodo.Completed = updatedTodo.Completed;
            existingTodo.CreatedAt = updatedTodo.CreatedAt == default
                ? existingTodo.CreatedAt
                : updatedTodo.CreatedAt;

            return Ok(existingTodo);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            _todos.Remove(todo);

            return NoContent();
        }
    }
}
