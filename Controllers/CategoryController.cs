using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTaskManager.Data;
using StudentTaskManager.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace StudentTaskManager.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int PageSize = 10;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index(int page = 1)
        {
            

            
            var allCategories = _context.Categories
                .OrderBy(c => c.Name) 
                .AsQueryable();

           
            int totalItems = allCategories.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);

         
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

           
            var categoriesOnPage = allCategories
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            
            var viewModel = new PaginatedTaskListViewModel
            {
                
                Categories = categoriesOnPage,
                PageNumber = page,
                TotalPages = totalPages,
                TotalTasks = totalItems
            };

            
            return View(viewModel); 
        }


        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            
            ModelState.Clear();

            
            if (string.IsNullOrEmpty(category.Name))
            {
                ModelState.AddModelError("Name", "Category Name is required.");
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges(); 
                return RedirectToAction(nameof(Index)); 
            }

            
            return View(category);
        }
        

        
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            
            ModelState.Clear();

            if (string.IsNullOrEmpty(category.Name))
            {
                ModelState.AddModelError("Name", "Category Name is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categories.Any(e => e.Id == category.Id))
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
            return View(category);
        }

        
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories
                .FirstOrDefault(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}