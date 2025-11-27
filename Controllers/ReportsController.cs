using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StudentTaskManager.Data;
using StudentTaskManager.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore; 
using System.Text.Json; 
using System; 

[Authorize]
public class ReportsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
       
        var allTasks = _context.Tasks.ToList();

        var tasksWithCategories = _context.Tasks
            .Include(t => t.Category) 
            .Where(t => t.Category != null) 
            .ToList();

        var monthlyData = new Dictionary<string, int>
        {
            {"Jan 2025", 15}, {"Feb 2025", 22}, {"Mar 2025", 18}, {"Apr 2025", 25}
        };

    
        var categoryData = tasksWithCategories
            .GroupBy(t => t.Category.Name)
            .Select(g => new {
                Name = g.Key,
                Count = g.Count()
            })
            .ToList();

        
        int totalTasksCount = allTasks.Count;
        int completedTasksCount = allTasks.Count(t => t.Status == "Completed");

        int overdueCount = allTasks.Count(t =>
            t.Status != "Completed" &&
            t.DueDate.Date < DateTime.Today);

        double completionRate = (totalTasksCount > 0)
            ? Math.Round(((double)completedTasksCount / totalTasksCount) * 100, 1)
            : 0.0;

        int onTimeCount = completedTasksCount; 

     
        var viewModel = new ReportViewModel
        {
            
            MonthlyLabels = monthlyData.Keys.ToList(),
            MonthlyCompletedCounts = monthlyData.Values.ToList(),

          
            CategoryNames = categoryData.Select(c => c.Name).ToList(),
            CategoryCounts = categoryData.Select(c => c.Count).ToList(),
            CategoryColors = new List<string> { "#7a6efe", "#4CAF50", "#FFC107", "#00BCD4" },

                
            CompletionRate = completionRate,
            OverdueCount = overdueCount,
            OnTimeCount = onTimeCount
        };

        return View(viewModel);
    }
}