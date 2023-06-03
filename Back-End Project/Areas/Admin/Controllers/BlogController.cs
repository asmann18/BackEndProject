using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]

    public class BlogController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        static int amount = 0;

        public BlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();

            return View(blogs);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogViewModel blogViewModel)
        {
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\blog\\" + "eheehe-" + amount + blogViewModel.Image.FileName;

            if (!ModelState.IsValid)
                return View();

            Blog blog = new()
            {
                Name = blogViewModel.Name,
                Redactor = blogViewModel.Redactor,
                Description = blogViewModel.Description,
                Image = "eheehe-" + amount + blogViewModel.Image.FileName
            };
            amount += 123;
            using (FileStream stream = new(path, FileMode.Create))
            {
                await blogViewModel.Image.CopyToAsync(stream);
            }
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog is null)
                return NotFound();

            BlogViewModel blogViewModel = new()
            {
                Name = blog.Name,
                Redactor = blog.Redactor,
                Description = blog.Description
            };
            return View(blogViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogViewModel blogViewModel)
        {
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\blog\\";

            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog is null)
                return NotFound();
            if (!ModelState.IsValid)
                return View();

            blog.Name = blogViewModel.Name;
            blog.Redactor = blogViewModel.Redactor;
            blog.Description = blogViewModel.Description;

            if (blogViewModel.Image is not null)
            {
                if (System.IO.File.Exists(path + blog.Image))
                {
                    System.IO.File.Delete(path + blog.Image);
                }
                using (FileStream stream = new(path + "ehehe-" + amount + blogViewModel.Image.FileName, FileMode.Create))
                {
                    await blogViewModel.Image.CopyToAsync(stream);
                }
                blog.Image = "ehehe-" + amount + blogViewModel.Image.FileName;
                amount += 123;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int id)
        {
            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog is null)
                return NotFound();

            return View(blog);


        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Blog? blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog is null)
                return NotFound();

            return View(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [ActionName("Delete")]
        public IActionResult DeleteBlog(int id)
        {
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\blog\\";

            var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog is null)
                return NotFound();

            if (System.IO.File.Exists(path + blog.Image))
            {
                System.IO.File.Delete(path + blog.Image);
            }
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
