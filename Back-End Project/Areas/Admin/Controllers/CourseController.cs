using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
    }
}
