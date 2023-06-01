using Back_End_Project.Models;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
 
        public IFormFile? Image { get; set; }
        [Required]
        public string Profession { get; set; }
        [Required]
        public string About { get; set; }
        [Required]
        public string Degree { get; set; }
        [Required]
        public int Experience { get; set; }
        [Required]
        public string Hobbies { get; set; }
        [Required]
        public string Faculty { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
     
        public int Phone { get; set; }


    }
}
