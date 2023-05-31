using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Migrations;
using Back_End_Project.Models;
using Back_End_Project.Models.ManyToMany;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        static int amount = 0;

        public EventController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Event> events = _context.Events.ToList();
            return View(events);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Speakers = _context.Speakers.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {
            ViewBag.Speakers = _context.Speakers.ToList();
            if (!ModelState.IsValid)
                return View();

            Event @event = new()
            {
                Name = eventViewModel.Name,
                StartTime = eventViewModel.StartTime,
                Duration = eventViewModel.Duration,
                Description = eventViewModel.Description,
                Image = "eheehe-" + amount + eventViewModel.Image.FileName
            };
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\speaker\\" + "eheehe-" + amount + eventViewModel.Image.FileName;
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            foreach (int id in eventViewModel.SpeakersIds)
            {
                EventSpeaker eventSpeaker = new()
                {
                    EventId = eventViewModel.Id,
                    SpeakerId = id
                };
                eventSpeakers.Add(eventSpeaker);

            }
            using (FileStream stream = new(path, FileMode.Create))
            {
                await eventViewModel.Image.CopyToAsync(stream);
            }
            @event.EventSpeakers = eventSpeakers;
            amount += 123;
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            Event? @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (@event == null)
                return NotFound();

            EventViewModel eventViewModel = new()
            {

                Name = @event.Name,
                StartTime = @event.StartTime,
                Duration = @event.Duration,
                Description = @event.Description

            };
            ViewBag.Speakers = _context.Speakers.ToList();
            return View(eventViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, EventViewModel eventViewModel)
        {
            Event? @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            ViewBag.Speakers = _context.Speakers.ToList();
            if (!ModelState.IsValid)
                    return View();

            if (@event == null)
                return NotFound();
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\speaker\\";


            if (eventViewModel.Image is not null)
            {
                if (System.IO.File.Exists(path + @event.Image))
                {
                    System.IO.File.Delete(path + @event.Image);
                }
                using (FileStream stream = new(path + "eheehe-" + amount + eventViewModel.Image.FileName, FileMode.Create))
                {
                    await eventViewModel.Image.CopyToAsync(stream);
                }
                @event.Image = "eheehe-" + amount + eventViewModel.Image.FileName;
                amount += 123;
            }
            @event.Name = eventViewModel.Name;
            @event.StartTime = eventViewModel.StartTime;
            @event.Duration = eventViewModel.Duration;
            @event.Description = eventViewModel.Description;
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
            if (eventViewModel.SpeakersIds != null)
            {

                foreach (int speakerid in eventViewModel.SpeakersIds)
                {
                    EventSpeaker eventspeaker = new()
                    {
                        EventId = eventViewModel.Id,
                        SpeakerId = speakerid

                    };
                    eventSpeakers.Add(eventspeaker);
                }
                @event.EventSpeakers = eventSpeakers;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Event? @event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (@event == null)
                return NotFound();

            return View(@event);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteEvent(int id)
        {
            string path = _webHostEnvironment.ContentRootPath + "wwwroot\\img\\speaker\\";

            Event? @event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (@event == null)
                return NotFound();

            if (System.IO.File.Exists(path + @event.Image))
            {
                System.IO.File.Delete(path + @event.Image);
            }

            _context.Events.Remove(@event);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
