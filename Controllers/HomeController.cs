using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTaskManager.Data;
using StudentTaskManager.Models;
using System.Linq;
using System.Text.Json; 

namespace StudentTaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
          
            var allTasks = _context.Tasks.Include(t => t.Category);

            int completedCount = allTasks.Count(t => t.Status == "Completed");
            int inProgressCount = allTasks.Count(t => t.Status == "In Progress");
            int pendingCount = allTasks.Count(t => t.Status == "Pending");
            int totalTasks = completedCount + inProgressCount + pendingCount;

           
            var model = new DashboardViewModel
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedCount,        
                PendingTasks = pendingCount,
                InProgressTasks = inProgressCount,
             
                RecentTasks = allTasks.OrderByDescending(t => t.DueDate).Take(5).ToList()
            };

            
            var statusData = new List<int> { completedCount, inProgressCount, pendingCount };
            if (totalTasks == 0)
            {
                statusData = new List<int> { 1, 1, 1 };
            }

            ViewBag.StatusDataJson = JsonSerializer.Serialize(statusData);

            
            var weeklyLabels = new List<string> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            var weeklyTaskCounts = new List<int> { 5, 8, 3, 7, 6, 9, 4 };

            ViewBag.WeeklyLabelsJson = JsonSerializer.Serialize(weeklyLabels);
            ViewBag.WeeklyCountsJson = JsonSerializer.Serialize(weeklyTaskCounts);

            return View(model);
        }
    }
}