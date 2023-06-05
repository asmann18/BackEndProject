using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Back_End_Project.Models.ManyToMany;
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

            ViewBag.Categories = await _context.Categories.Include(x=>x.CategoryCourses).ToListAsync();

            return View(course);

    }
        public async Task<IActionResult> filterCourses(int id)
        {
            Category? category = await _context.Categories.Include(_ => _.CategoryCourses).FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                return NotFound();
            List<Course> courses = new List<Course>();
            foreach (CategoryCourse categoryCourse in category.CategoryCourses)
            {
                Course? course = await _context.Courses.FirstOrDefaultAsync(c=>c.Id==categoryCourse.CourseId );
                courses.Add(course);
            }

            return View(courses);
        } 
    }
}
