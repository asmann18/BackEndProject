using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_Project.Controllers
{
    public class CoursesController : Controller
    {

        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        public async  Task<IActionResult> Index()
        {
            List<Course> courses = await _context.Courses.Include(x=>x.CategoryCourses).ToListAsync();
            return View(courses);
        }
    public async Task<IActionResult> Detail(int id)
    {
            Course? course = await _context.Courses.Include(_ => _.CategoryCourses).FirstOrDefaultAsync(c=>c.Id==id);
            if (course is null)
                return BadRequest();

            return View(course);

    }
    }
}
