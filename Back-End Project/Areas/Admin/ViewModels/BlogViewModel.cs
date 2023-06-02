using Microsoft.Build.Framework;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public string Redactor { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
