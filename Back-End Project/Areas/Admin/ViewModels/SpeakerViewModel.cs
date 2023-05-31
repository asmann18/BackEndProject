using Back_End_Project.Models.ManyToMany;
using Microsoft.Build.Framework;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class SpeakerViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Profession { get; set; }
        public IFormFile? Image { get; set; }
  

    }
}
