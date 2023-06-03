using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Back_End_Project.Models.ManyToMany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.IO;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]

    public class CourseController : Controller
    {
        private static double amount = 0;
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
            ViewBag.Categories = _context.Categories.ToList();

            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create(CourseViewModel courseViewModel)
        {

            string path = _webHostEnvironment.ContentRootPath + "\\wwwroot\\img\\course\\" + "eheehe-" + amount + courseViewModel.Image.FileName;

            ViewBag.Categories = _context.Categories.ToList();

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
            


            }
            ;
            

            List<CategoryCourse> categoryCourses = new List<CategoryCourse>();

            foreach (int id in courseViewModel.CategoryIds)
            {
                CategoryCourse categoryCourse = new()
                {
                    CourseId = courseViewModel.Id,
                    CategoryId = id

                };
               categoryCourses.Add(categoryCourse);
                Category? category=await _context.Categories.Include(x=>x.CategoryCourses).FirstOrDefaultAsync(c=>c.Id==id);
                if (category is null)
                    return BadRequest();
                category.CategoryCourses.Add(categoryCourse);
                
            }
            course.CategoryCourses = categoryCourses;
            amount += 123;
            await _context.Courses.AddAsync(course);


            using (FileStream stream = new(path, FileMode.Create))
            {
                await courseViewModel.Image.CopyToAsync(stream);
            };
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Course? course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
                return NotFound();

            return View(course);


        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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





