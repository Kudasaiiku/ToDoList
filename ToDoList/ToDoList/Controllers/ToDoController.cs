using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToDoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ToDo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemModel>>> GetToDoItems()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await _context.ToDoItem.Where(item => item.UserId == userId).ToListAsync();
        }

        // GET: api/ToDo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemModel>> GetToDoItem(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var toDoItem = await _context.ToDoItem.FindAsync(id);

            if (toDoItem == null || toDoItem.UserId != userId)
            {
                return NotFound();
            }

            return toDoItem;
        }

        // POST: api/ToDo
        [HttpPost]
        public async Task<ActionResult<ToDoItemModel>> PostToDoItem(ToDoItemModel toDoItem)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            toDoItem.UserId = userId;

            _context.ToDoItem.Add(toDoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoItem", new { id = toDoItem.Id }, toDoItem);
        }

        // PUT: api/ToDo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(int id, ToDoItemModel toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return BadRequest();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (toDoItem.UserId != userId)
            {
                return Forbid();
            }

            _context.Entry(toDoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ToDo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var toDoItem = await _context.ToDoItem.FindAsync(id);

            if (toDoItem == null || toDoItem.UserId != userId)
            {
                return NotFound();
            }

            _context.ToDoItem.Remove(toDoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoItemExists(int id)
        {
            return _context.ToDoItem.Any(e => e.Id == id);
        }
    }
}
