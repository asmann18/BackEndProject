using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back_End_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Course> courses = _context.Courses.ToList();
            return View(courses);
        }
    }
}
