using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]

    public class SubscribeController : Controller
    {
        private readonly AppDbContext _context;

        public SubscribeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Subscribe> subscribes = await _context.Subscribes.ToListAsync();
            return View(subscribes);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Subscribe? subscribe = _context.Subscribes.FirstOrDefault(x => x.Id == id);
            if (subscribe is null)
                return NotFound();

            return View(subscribe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [ActionName("Delete")]
        public IActionResult DeleteSubscribe(int id)
        {

            Subscribe? subscribe = _context.Subscribes.FirstOrDefault(x => x.Id == id);
            if (subscribe is null)
                return NotFound();

            _context.Subscribes.Remove(subscribe);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));

        }
    }
}
