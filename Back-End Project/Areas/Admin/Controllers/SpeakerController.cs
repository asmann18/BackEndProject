using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpeakerController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        static int amount = 0;

        public SpeakerController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\speaker\\";
            ViewBag.path = path;
            List<Speaker> speakers = _context.Speakers.ToList();

            return View(speakers);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerViewModel speakerViewModel)
        {

            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\speaker\\" + "eheehe-" + amount + speakerViewModel.Image.FileName;

            if (!ModelState.IsValid)
                return View();

            Speaker speaker = new()
            {
                Name = speakerViewModel.Name,
                Profession = speakerViewModel.Profession,
                Image = "eheehe-" + amount + speakerViewModel.Image.FileName
            };

            await _context.Speakers.AddAsync(speaker);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await speakerViewModel.Image.CopyToAsync(stream);
            };
            await _context.SaveChangesAsync();
            amount += 123;

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int id)
        {
            Speaker speaker = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == id);

            if (speaker is null)
                return NotFound();

            SpeakerViewModel speakerViewModel = new()
            {
                Name = speaker.Name,
                Profession = speaker.Profession
            };
            return View(speakerViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,SpeakerViewModel speakerViewModel)
        {
            Speaker speaker = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == id);

            if (speaker is null)
                return NotFound();

            if (speakerViewModel.Image is not null)
            {

                string path = _webHostEnvironment.ContentRootPath + "\\wwwroot\\img\\speaker\\" ;

                if (System.IO.File.Exists(path+ speaker.Image))
                {

                    System.IO.File.Delete(path+ speaker.Image);
                }
                using(FileStream stream = new(path + "eheehe-" + amount +speakerViewModel.Image.FileName, FileMode.Create))
                {
                    await speakerViewModel.Image.CopyToAsync(stream);
                }
                speaker.Image = "eheehe-" + amount + speakerViewModel.Image.FileName;
            }
            speaker.Name= speakerViewModel.Name;
            speaker.Profession=speakerViewModel.Profession;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        public  IActionResult Delete(int id)
        {
            Speaker? speaker= _context.Speakers.FirstOrDefault(s => s.Id == id);
            if (speaker is null)
                return NotFound();

            return View(speaker);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteSpeaker(int id)
        {
            Speaker? speaker = _context.Speakers.FirstOrDefault(s => s.Id == id);
                if(speaker is null)
                return NotFound();

            string path = _webHostEnvironment.ContentRootPath + "\\wwwroot\\img\\speaker\\";


            if (System.IO.File.Exists(path + speaker.Image))
            {

                System.IO.File.Delete(path + speaker.Image);
            }

            _context.Speakers.Remove(speaker);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
