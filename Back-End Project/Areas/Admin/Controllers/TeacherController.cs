using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Back_End_Project.Utilits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]

    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        static int amount = 0;   

        public TeacherController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            
        }

        public IActionResult Index()
        {
            List<Teacher> teachers = _context.Teachers.ToList();
            return View(teachers);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherViewModel teacherViewModel)
        {
            if (!ModelState.IsValid)
                return View();
            if (!teacherViewModel.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "File type is not image .");
                return View();
            }
            if (!teacherViewModel.Image.CheckFileSize(1500))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 1 mb-dan kicik olmalidir.");
                return View();
            }

            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\teacher\\" + "eheehe-" + amount + teacherViewModel.Image.FileName;


            Teacher teacher = new()
            {
                Name = teacherViewModel.Name,
                Profession = teacherViewModel.Profession,
                Hobbies = teacherViewModel.Hobbies,
                Experience = teacherViewModel.Experience,
                About = teacherViewModel.About,
                Degree = teacherViewModel.Degree,
                Faculty = teacherViewModel.Faculty,
                Image = "eheehe-" + amount + teacherViewModel.Image.FileName,
                Mail=teacherViewModel.Mail,
                Phone=teacherViewModel.Phone
            };
             using(FileStream stream = new(path, FileMode.Create))
            {
                await teacherViewModel.Image.CopyToAsync(stream);
            }

            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            Teacher? teacher=await _context.Teachers.FirstOrDefaultAsync(t=>t.Id == id);
            if (teacher == null)
                return NotFound();

            TeacherViewModel teacherViewModel = new()
            {
                About = teacher.About,
                Degree = teacher.Degree,
                Experience = teacher.Experience,
                Faculty = teacher.Faculty,
                Hobbies = teacher.Hobbies,
                Mail = teacher.Mail,
                Name = teacher.Name,
                Phone = teacher.Phone,
                Profession = teacher.Profession
            };
            return View(teacherViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id,TeacherViewModel teacherViewModel)
        {
            Teacher? teacher= await _context.Teachers.FirstOrDefaultAsync(t=>t.Id == id);
            if (teacher == null)
                return NotFound();
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\teacher\\";

            if (teacherViewModel.Image is not null)
            {
                if (!teacherViewModel.Image.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("Image", "File type is not image .");
                    return View();
                }
                if (!teacherViewModel.Image.CheckFileSize(1500))
                {
                    ModelState.AddModelError("Image", "Faylin hecmi 1 mb-dan kicik olmalidir.");
                    return View();
                }
                if (System.IO.File.Exists(path + teacher.Image))
                {
                    System.IO.File.Delete(path + teacher.Image);
                }
                using(FileStream stream = new(path + "ehehehe-" + amount + teacherViewModel.Image.FileName, FileMode.Create))
                {
                    await teacherViewModel.Image.CopyToAsync(stream);
                }
                teacher.Image = "ehehehe-" + amount + teacherViewModel.Image.FileName;
            }
            teacher.Profession=teacherViewModel.Profession;
            teacher.Name = teacherViewModel.Name;
            teacher.Phone = teacherViewModel.Phone;
            teacher.Faculty=teacherViewModel.Faculty; 
            teacher.About=teacherViewModel.About;
            teacher.Hobbies=teacherViewModel.Hobbies;
            teacher.Degree = teacherViewModel.Degree;
            teacher.Mail = teacherViewModel.Mail;
            teacher.Experience = teacherViewModel.Experience;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Teacher? teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);
            if (teacher is null)
                return NotFound();

            return View(teacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteTeacher(int id)
        {
            Teacher? teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);
            if (teacher is null)
                return NotFound();
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\teacher\\";


            if (System.IO.File.Exists(path + teacher.Image))
            {
                System.IO.File.Delete(path + teacher.Image);
            }
            _context.Teachers.Remove(teacher);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Detail(int id)
        {
            Teacher? teacher =await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher is null)
                return NotFound();

            return View(teacher);
        }
    }
}
