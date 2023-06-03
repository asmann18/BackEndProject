using Back_End_Project.Areas.Admin.ViewModels;
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
            List<Course> Courses = _context.Courses.ToList();
            List<Event> Events = _context.Events.ToList();
            List<Blog> Blogs = _context.Blogs.ToList();

            HomeControllerViewModel homeControllerViewModel = new HomeControllerViewModel(Courses, Events, Blogs);
          
            return View(homeControllerViewModel);
        }
    }
}
