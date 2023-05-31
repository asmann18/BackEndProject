using Back_End_Project.Models;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public IFormFile? Image { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int ClassDuration { get; set; }
        [Required]
        public string SkillLevel { get; set; }
        [Required]
        public byte StudentCount { get; set; }
        [Required]
        public string Assesments { get; set; }

        [Required]
        public int[] CategoryIds { get; set; }
    }
}
