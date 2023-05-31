using Back_End_Project.Models.ManyToMany;
using Microsoft.Build.Framework;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile? Image { get; set; }

        public int[] SpeakersIds { get; set; }
    }
}
