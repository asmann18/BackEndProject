using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    [Area("Admin")]
    public class SkillController : Controller
    {
        private readonly AppDbContext _context;

        public SkillController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            List<Skill> skills = _context.Skills.Include(x=>x.Teacher).ToList();
            return View(skills);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers =await _context.Teachers.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SkillViewModel skillViewModel)
        {
            ViewBag.Teachers =await _context.Teachers.ToListAsync();
            if (!ModelState.IsValid)
                return View();


            Skill skill = new()
            {
                Name=skillViewModel.Name,
                Point=skillViewModel.Point, 
                TeacherId=skillViewModel.TeacherId
            };

            await _context.Skills.AddAsync(skill);
            Teacher? teacher=await _context.Teachers.FirstOrDefaultAsync(t => t.Id == skillViewModel.TeacherId);
            if (teacher is null)
                return View();

            teacher.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            Skill? skill=await _context.Skills.FirstOrDefaultAsync(t => t.Id == id);
            if (skill is null)
                return NotFound();
            ViewBag.Teachers = await _context.Teachers.ToListAsync();

            SkillViewModel skillViewModel = new()
            {
                Name = skill.Name,
                Point = skill.Point,
                TeacherId = skill.TeacherId
            };
            return View(skillViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,SkillViewModel skillViewModel)
        {
            ViewBag.Teachers = await _context.Teachers.ToListAsync();

            Skill? skill = await _context.Skills.FirstOrDefaultAsync(t => t.Id == id);
            if (skill is null)
                return NotFound();

            if (!ModelState.IsValid)
                return NotFound();
            
            if (skill.TeacherId != skillViewModel.TeacherId)
            {
                Teacher? teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == skill.TeacherId);
                teacher.Skills.Remove(skill);
                Teacher? teacher2 = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == skillViewModel.TeacherId);
                if (teacher2 is null)
                    return View();

                teacher2.Skills.Add(skill);
            }
            skill.Name = skillViewModel.Name;
            skill.Point = skillViewModel.Point;
            skill.TeacherId = skillViewModel.TeacherId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Delete(int id)
        {
            Skill? skill = _context.Skills.Include(x=>x.Teacher).FirstOrDefault(s => s.Id == id);
            if (skill is null)
                return NotFound();
            return View(skill);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteSkill(int id)
        {
            Skill? skill = _context.Skills.Include(x => x.Teacher).FirstOrDefault(s => s.Id == id);
            if (skill is null)
                return NotFound();

            _context.Skills.Remove(skill);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

    }
}
