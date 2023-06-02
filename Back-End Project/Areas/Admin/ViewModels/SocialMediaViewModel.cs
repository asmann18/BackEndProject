using Back_End_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class SocialMediaViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Icon { get; set; }
        [Required]
        public string Path { get; set; }

        [Required]
        public int TeacherId { get; set; }
    }
}




