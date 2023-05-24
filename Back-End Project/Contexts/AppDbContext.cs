using Back_End_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Back_End_Project.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public List<Course> Courses { get; set; }
    }
}
