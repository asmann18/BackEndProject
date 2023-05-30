using Back_End_Project.Models.ManyToMany;
using System.ComponentModel.DataAnnotations;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public ICollection<CategoryCourse> categoryCourses { get; set; }
    }
}
