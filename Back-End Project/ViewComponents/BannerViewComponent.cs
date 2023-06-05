using Back_End_Project.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Back_End_Project.ViewComponents
{
    public class BannerViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public BannerViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            ViewBag.Name = name;

            return View();
        }
    }
}
