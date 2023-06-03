using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Back_End_Project.Models.ManyToMany;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_Project.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Event> events =await _context.Events.ToListAsync();
            return View(events);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Event? eventt = await _context.Events.Include(x=>x.EventSpeakers).ThenInclude(y=>y.Speaker).FirstOrDefaultAsync(e => e.Id == id);
            if (eventt is null)
                return NotFound();
            List<Speaker> speakers = new List<Speaker>();
            foreach (EventSpeaker eventSpeaker in eventt.EventSpeakers)
            {
                Speaker speaker = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == eventSpeaker.SpeakerId);
                speakers.Add(speaker);
            }
            ViewBag.Speakers=speakers;

            return View(eventt);

        }
    }
}
