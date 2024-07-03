using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string status, int? priority)
        {
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Пользователь";
            ViewBag.UserName = userName;

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var itemsQuery = _context.ToDoItem.Where(item => item.UserId == userId);

            var items = await itemsQuery.ToListAsync();

            if (!string.IsNullOrEmpty(status))
            {
                string isCompletedStr = status.Equals("true", StringComparison.OrdinalIgnoreCase) ? "Выполнена" : "Невыполнена";
                items = items.Where(item => item.IsCompleted.Equals(isCompletedStr, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (priority.HasValue)
            {
                items = items.Where(item => item.Priority == priority.Value).ToList();
            }

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(string title, string description, bool isCompleted, DateTime dueDate, int priority)
        {
            var isCompletedStr = isCompleted ? "Выполнена" : "Невыполнена";

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            var task = new ToDoItemModel
            {
                Title = title,
                Description = description,
                IsCompleted = isCompletedStr,
                DueDate = dueDate,
                Priority = priority,
                UserId = userId
            };

            _context.ToDoItem.Add(task);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.ToDoItems = await _context.ToDoItem.CountAsync(tasks => tasks.UserId == userId);
                await _context.SaveChangesAsync();
            }

            var priorityRecord = new PriorityModel
            {
                ToDoItemId = task.Id,
                Level = priority
            };

            _context.Priority.Add(priorityRecord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(int id, string title, string description, bool isCompleted, DateTime dueDate, int priority)
        {
            var task = await _context.ToDoItem.FindAsync(id);

            var isCompletedStr = isCompleted ? "Выполнена" : "Невыполнена";

            task.Title = title;
            task.Description = description;
            task.IsCompleted = isCompletedStr;
            task.DueDate = dueDate;
            task.Priority = priority;

            var priorityModel = await _context.Priority.FirstOrDefaultAsync(priority => priority.ToDoItemId == id);
            if (priorityModel != null)
            {
                priorityModel.Level = priority;
                _context.Priority.Update(priorityModel);
            }
            else
            {
                _context.Priority.Add(new PriorityModel { ToDoItemId = id, Level = priority });
            }

            _context.ToDoItem.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.ToDoItem.FindAsync(id);

            if (task != null)
            {
                var userId = task.UserId;
                var user = await _context.Users.FindAsync(userId);

                _context.ToDoItem.Remove(task);

                var priority = await _context.Priority.FirstOrDefaultAsync(priority => priority.ToDoItemId == id);
                if (priority != null)
                {
                    _context.Priority.Remove(priority);
                }

                await _context.SaveChangesAsync();

                if (user != null)
                {
                    user.ToDoItems = await _context.ToDoItem.CountAsync(t => t.UserId == userId);

                    _context.Users.Update(user);

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }
    }
}