using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Blog? blog=await _context.Blogs.FirstOrDefaultAsync(b=>b.Id==id);
            if (blog is null)
                return NotFound();

            return View(blog);
        }
    }
}
