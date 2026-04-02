using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using System.Security.Cryptography;
using System.Text;


namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private static readonly List<Todo> _todos = new();

        // Gumagawa ng SHA-256 hash gamit yung todo data at previous hash
        private static string ComputeHash(Todo todo, string previousHash)
        {
            using var sha256 = SHA256.Create();
            var rawData = $"{todo.Title}|{todo.Completed}|{todo.CreatedAt:o}|{previousHash}";
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexString(bytes);
        }

        private static void RebuildChainFrom(int startIndex)
        {
            for (int i = startIndex; i < _todos.Count; i++)
            {
                var previousHash = i == 0 ? "GENESIS" : _todos[i - 1].Hash;
                _todos[i].PreviousHash = previousHash;
                _todos[i].Hash = ComputeHash(_todos[i], previousHash);

            }
        }



        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_todos);
        }

        //updated post to generate prevhash and hash

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
            RebuildChainFrom(_todos.Count - 1);


            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }


        // updated put so when a todo changes its hash is recalculated.
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Todo updatedTodo)
        {
            var index = _todos.FindIndex(t => t.Id == id);

            if (index == -1)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(updatedTodo.Title))
            {
                return BadRequest("Title is required.");
            }

            var existingTodo = _todos[index];

            existingTodo.Title = updatedTodo.Title;
            existingTodo.Completed = updatedTodo.Completed;
            existingTodo.CreatedAt = updatedTodo.CreatedAt == default
                ? existingTodo.CreatedAt
                : updatedTodo.CreatedAt;

            _todos[index] = existingTodo;
            RebuildChainFrom(index);


            return Ok(existingTodo);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = _todos.FindIndex(t => t.Id == id);

            if (index == -1)
            {
                return NotFound();
            }

            _todos.RemoveAt(index);

            if (_todos.Count > 0 && index < _todos.Count)
            {
                RebuildChainFrom(index);
            }

            return NoContent();
        }


        //added to verify endpoint. to match if the prev hash is matches. and valid
        [HttpGet("verify")]
        public IActionResult Verify()
        {
            if (_todos.Count == 0)
            {
                return Ok(new { message = "Chain valid", valid = true });
            }

            for (int i = 0; i < _todos.Count; i++)
            {
                var currentTodo = _todos[i];
                // Dapat tugma ang previous hash sa hash ng naunang todo
                var expectedPreviousHash = i == 0 ? "GENESIS" : _todos[i - 1].Hash;

                if (currentTodo.PreviousHash != expectedPreviousHash)
                {
                    return Conflict(new
                    {
                        message = "Chain tampered",
                        valid = false
                    });
                }

                // Recompute para macheck kung may nabago sa data
                var recalculatedHash = ComputeHash(currentTodo, expectedPreviousHash);

                if (currentTodo.Hash != recalculatedHash)
                {
                    return Conflict(new
                    {
                        message = "Chain tampered",
                        valid = false
                    });
                }
            }

            return Ok(new
            {
                message = "Chain valid",
                valid = true
            });
        }

    }
}
