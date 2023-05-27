using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Back_End_Project.Areas.Admin.ViewModels;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        
        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            List<Course> courses = _context.Courses.ToList();
            return View(courses);
        }
        public IActionResult Create()
        {
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseViewModel courseViewModel)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
