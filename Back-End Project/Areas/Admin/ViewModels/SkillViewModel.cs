using System.ComponentModel.DataAnnotations;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class SkillViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 100)]
        public int Point { get; set; }
        [Required]
        public int TeacherId { get; set; }
    }
}
