using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        static double amount = 0;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public CourseController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.root = _webHostEnvironment.WebRootPath;
            List<Course> courses = _context.Courses.ToList();
            return View(courses);
        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create(CourseViewModel courseViewModel)
        {

            string path = _webHostEnvironment.ContentRootPath + "\\wwwroot\\img\\course\\" + "eheehe-" + amount + courseViewModel.Image.FileName;


            Course course = new()
            {
                Name = courseViewModel.Name,
                Image = "eheehe-" + amount + courseViewModel.Image.FileName,
                Desc = courseViewModel.Desc,
                Price = courseViewModel.Price,
                Assesments = courseViewModel.Assesments,
                SkillLevel = courseViewModel.SkillLevel,
                StartTime = courseViewModel.StartTime,
                StudentCount = courseViewModel.StudentCount,
                ClassDuration = courseViewModel.ClassDuration,
                Duration = courseViewModel.Duration,
                Id = courseViewModel.Id,


            }
            ;

            amount += 123;
            _context.Courses.Add(course);
            _context.SaveChanges();

            using (FileStream stream = new(path, FileMode.Create))
            {
                await courseViewModel.Image.CopyToAsync(stream);
            };

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            Course? course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
                return NotFound();

            return View(course);


        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            Course? course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                return NotFound();

            string path = _webHostEnvironment.ContentRootPath + "\\wwwroot\\img\\course\\" + course.Image;

            if (System.IO.File.Exists(path))
            {

                System.IO.File.Delete(path);
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Update(int id)
        {
            Course? course=await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
                return NotFound();

            CourseViewModel courseViewModel = new()
            {

                Name = course.Name,
                Desc = course.Desc,
                Price = (int)course.Price,
                Assesments = course.Assesments,
                SkillLevel = course.SkillLevel,
                StartTime = course.StartTime,
                StudentCount = course.StudentCount,
                ClassDuration = course.ClassDuration,
                Duration = course.Duration,
              

            };


            return View(courseViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,CourseViewModel courseViewModel)
        {

            Course? course= await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View();
            string path = _webHostEnvironment.ContentRootPath + "\\wwwroot\\img\\course\\" ;
            string path2 = _webHostEnvironment.ContentRootPath + "\\wwwroot\\img\\course\\" + "eheehe-" + amount;

            if (courseViewModel.Image != null)
            {
                if(System.IO.File.Exists(path+course.Image))
                    System.IO.File.Delete(path+course.Image);
                course.Image = "eheehe-" + amount+courseViewModel.Image.FileName;

                using(FileStream stream = new(path2+ courseViewModel.Image.FileName, FileMode.Create))
                {
                    await courseViewModel.Image.CopyToAsync(stream);
                }
            amount += 123;
            }
            course.Name = courseViewModel.Name;
            course.Assesments = courseViewModel.Assesments;
            course.Desc = courseViewModel.Desc;
            course.Price = courseViewModel.Price;
            course.StartTime = courseViewModel.StartTime;
            course.ClassDuration= courseViewModel.ClassDuration;
            course.Duration = courseViewModel.Duration;
            course.SkillLevel = courseViewModel.SkillLevel;
            course.StudentCount=courseViewModel.StudentCount;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}





