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

        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Speaker> Speakers { get; set; } = null!;
        public DbSet<EventSpeaker> EventSpeakers { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<SocialMedia> SocialMedias { get; set; } = null!;



        //Key olaraq bunlarida yazmalisan migration edende - @Huseyn
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<EventSpeaker>()
        //        .HasKey(e => new { e.EventId, e.SpeakerId});
        //}
    }
}
