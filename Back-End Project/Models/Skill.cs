using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Back_End_Project.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0,100)]
        public int Point { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
