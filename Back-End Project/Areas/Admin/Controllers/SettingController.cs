using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Setting> settings = await _context.Settings.ToListAsync();
            return View(settings);
        }
        public async Task<IActionResult> Update(int id)
        {
            Setting? setting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (setting is null)
                return NotFound();
            SettingViewModel settingViewModel = new()
            {
                Key = setting.Key,
                Value=setting.Value
            };
            return View(settingViewModel);
        }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id,SettingViewModel settingViewModel)
    {
            if (!ModelState.IsValid)
                return View();
            Setting? setting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (setting is null)
                return NotFound();
            setting.Key = settingViewModel.Key;
            setting.Value=settingViewModel.Value;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
    }

        public IActionResult Delete(int id)
        {
            Setting? setting = _context.Settings.FirstOrDefault(x => x.Id == id);
            if (setting is null)
                return NotFound();

            return View(setting);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteSetting(int id)
        {
            Setting? setting =_context.Settings.FirstOrDefault(x => x.Id == id);
            if (setting is null)
                return NotFound();
            _context.Settings.Remove(setting);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingViewModel settingViewModel)
        {
            if (!ModelState.IsValid)
                return NotFound();
            Setting? settingKey=await _context.Settings.FirstOrDefaultAsync(c=>c.Key==settingViewModel.Key);
            if (settingKey is not null)
            {
                ModelState.AddModelError("Key", "Key is already exist");
                return View();
            }
            Setting setting = new()
            {
                Key = settingViewModel.Key,
                Value = settingViewModel.Value
            };
            await _context.Settings.AddAsync(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
