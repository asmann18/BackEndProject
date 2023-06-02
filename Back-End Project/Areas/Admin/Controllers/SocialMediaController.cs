using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialMediaController : Controller
    {
        private readonly AppDbContext _context;

        public SocialMediaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<SocialMedia> socialMedias = await _context.SocialMedias.Include(s => s.Teacher).ToListAsync();
            return View(socialMedias);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers = await _context.Teachers.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SocialMediaViewModel socialMediaViewModel)
        {
            ViewBag.Teachers = await _context.Teachers.ToListAsync();
            if (!ModelState.IsValid)
                return View();

            Teacher? teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == socialMediaViewModel.TeacherId);
            if (teacher is null)
                return View();

            SocialMedia socialMedia = new()
            {
                Icon = socialMediaViewModel.Icon,
                Path = socialMediaViewModel.Path,
                TeacherId = socialMediaViewModel.TeacherId
            };
            await _context.SocialMedias.AddAsync(socialMedia);
            teacher.SocialMedias.Add(socialMedia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Teachers = await _context.Teachers.ToListAsync();
            SocialMedia? socialMedia = await _context.SocialMedias.FirstOrDefaultAsync(t => t.Id == id);
            if (socialMedia is null)
                return NotFound();
            SocialMediaViewModel socialMediaViewModel = new()
            {
                Path = socialMedia.Path,
                Icon = socialMedia.Icon,
                TeacherId = socialMedia.TeacherId
            };
            return View(socialMediaViewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SocialMediaViewModel socialMediaViewModel)
        {
            ViewBag.Teachers = await _context.Teachers.ToListAsync();
            SocialMedia? socialMedia = await _context.SocialMedias.FirstOrDefaultAsync(t => t.Id == id);
            if (socialMedia is null)
                return NotFound();
            if (!ModelState.IsValid)
                return View();

            if (socialMedia.TeacherId != socialMediaViewModel.TeacherId)
            {
                Teacher teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == socialMedia.TeacherId);
                teacher.SocialMedias.Remove(socialMedia);
                Teacher teacher2 = await _context.Teachers.Include(x=>x.SocialMedias).FirstOrDefaultAsync(t => t.Id == socialMediaViewModel.TeacherId);
                if (teacher2 is null)
                    return View();
                teacher2.SocialMedias.Add(socialMedia);

            }
            socialMedia.Icon = socialMediaViewModel.Icon;
            socialMedia.Path = socialMediaViewModel.Path;
            socialMedia.TeacherId = socialMediaViewModel.TeacherId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            SocialMedia? socialMedia = _context.SocialMedias.Include(x=>x.Teacher).FirstOrDefault(s => s.Id == id);
            if (socialMedia is null)
                return NotFound();
            return View(socialMedia);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteSocialMedia(int id)
        {
            SocialMedia? socialMedia = _context.SocialMedias.Include(x => x.Teacher).FirstOrDefault(s => s.Id == id);
            if (socialMedia is null)
                return NotFound();

            _context.SocialMedias.Remove(socialMedia);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
