using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentTaskManager.Data;
using StudentTaskManager.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace StudentTaskManager.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int PageSize = 10;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }


      
        public IActionResult Index(string searchString, string statusFilter, int page = 1)
        {
           

           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasksQuery = _context.Tasks
                .Include(t => t.Category)
                .Where(t => t.UserId == userId) 
                .AsQueryable();

            
            if (!string.IsNullOrEmpty(searchString))
            {
                
                tasksQuery = tasksQuery.Where(s => s.Title.Contains(searchString)
                                       || (s.Category != null && s.Category.Name.Contains(searchString)));
            }

          
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                tasksQuery = tasksQuery.Where(t => t.Status == statusFilter);
            }

          

          
            int totalItems = tasksQuery.Count();

          
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);

           
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

          
            var tasksOnPage = tasksQuery
                .OrderByDescending(t => t.DueDate)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

           
            var viewModel = new PaginatedTaskListViewModel
            {
                Tasks = tasksOnPage,
                PageNumber = page,
                TotalPages = totalPages,
                TotalTasks = totalItems 
            };

           
            ViewBag.CurrentStatusFilter = statusFilter;
            ViewBag.CurrentSearchString = searchString;
            return View(viewModel);
        }


        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskModel taskModel)
        {
            ModelState.Clear();
            if (taskModel.CategoryId == 0) { ModelState.AddModelError("CategoryId", "Please select a category."); }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(taskModel.Status)) { taskModel.Status = "Pending"; }

                taskModel.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

                _context.Add(taskModel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", taskModel.CategoryId);
            return View(taskModel);
        }

       

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null) { return NotFound(); }

            
            var task = _context.Tasks
                .Include(t => t.Category)
                .FirstOrDefault(m => m.Id == id);

            if (task == null) { return NotFound(); }

           
            _context.Entry(task)
                .Collection(t => t.Subtasks)
                .Query()
                .OrderBy(s => s.Id)
                .Load();

            

            var viewModel = new TaskDetailViewModel
            {
                Task = task,
                Subtasks = task.Subtasks.ToList(),
                NewSubtask = new Subtask { TaskId = task.Id }
            };
            return View(viewModel);
        }

        [HttpPost] 
        [ValidateAntiForgeryToken]
        public IActionResult Details(TaskDetailViewModel viewModel)
        {
            
            if (viewModel.NewSubtask == null || string.IsNullOrWhiteSpace(viewModel.NewSubtask.Title) || viewModel.NewSubtask.TaskId <= 0)
            {
               
                return RedirectToAction(nameof(Details), new { id = viewModel.Task?.Id });
            }

           
            var subtaskToSave = viewModel.NewSubtask;
            subtaskToSave.IsCompleted = false;

           
            _context.Subtasks.Add(subtaskToSave);
            _context.SaveChanges();

           
            return RedirectToAction(nameof(Details), new { id = subtaskToSave.TaskId });
        }


       
        public IActionResult ToggleSubtask(int id)
        {
            var subtask = _context.Subtasks.Find(id);

            if (subtask != null)
            {
               
                subtask.IsCompleted = !subtask.IsCompleted;
                _context.SaveChanges();

               
                var parentTask = _context.Tasks
                    .Include(t => t.Subtasks)
                    .FirstOrDefault(t => t.Id == subtask.TaskId);

                if (parentTask != null)
                {
                    
                    bool allCompleted = parentTask.Subtasks.All(s => s.IsCompleted);

                    if (allCompleted && parentTask.Status != "Completed")
                    {
                       
                        parentTask.Status = "Completed";
                        _context.Update(parentTask);
                        _context.SaveChanges();
                    }
                    else if (!allCompleted && parentTask.Status == "Completed")
                    {
                       
                        parentTask.Status = "In Progress";
                        _context.Update(parentTask);
                        _context.SaveChanges();
                    }
                }
            }

            
            return RedirectToAction(nameof(Details), new { id = subtask.TaskId });
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var taskModel = _context.Tasks.Find(id);
            if (taskModel == null)
            {
                return NotFound();
            }

            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", taskModel.CategoryId);
            return View(taskModel); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TaskModel taskModel)
        {
            if (id != taskModel.Id)
            {
                return NotFound();
            }

            
            ModelState.Clear();
            ModelState.Remove("Category");

            if (taskModel.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Please select a category.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskModel);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Tasks.Any(e => e.Id == taskModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", taskModel.CategoryId);
            return View(taskModel);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

        
            var taskModel = _context.Tasks
                .Include(t => t.Category)
                .FirstOrDefault(m => m.Id == id);
            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel); 
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var taskModel = _context.Tasks.Find(id);
            if (taskModel != null)
            {
                _context.Tasks.Remove(taskModel);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
       
        
    }
}