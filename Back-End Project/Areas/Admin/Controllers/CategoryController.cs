using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Moderator")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();

            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        {
            if (categoryViewModel.Name==null)
                return View();

            Category category = new()
            {
                Name = categoryViewModel.Name
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            

            return RedirectToAction(nameof(Index)) ;
               
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Category? category =_context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null)
                return NotFound();

            return View(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index)) ;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                return NotFound();

            CategoryViewModel categoryViewModel = new()
            {
                Name = category.Name
            };
            return View(categoryViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update (int id,CategoryViewModel categoryViewModel)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                return NotFound();

            category.Name=categoryViewModel.Name;
             await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
