using Back_End_Project.Models;
using Back_End_Project.Models.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;


namespace Back_End_Project.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<CategoryCourse> CategoryCourses { get; set; } = null!;

    }
}
