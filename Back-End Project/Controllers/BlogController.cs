﻿using Microsoft.AspNetCore.Mvc;

namespace Back_End_Project.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}