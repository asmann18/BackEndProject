using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_Project.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Teacher> teachers = await _context.Teachers.ToListAsync();
            

            return View(teachers);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Teacher? teacher=await _context.Teachers.Include(x=>x.Skills).Include(y=>y.SocialMedias).FirstOrDefaultAsync(t=>t.Id==id);
            if(teacher is null)
                return NotFound();

            ViewBag.Skills =teacher.Skills.ToList();
            ViewBag.SocialMedias =teacher.SocialMedias.ToList();
            return View(teacher);
        }
    }
}
