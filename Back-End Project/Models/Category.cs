

using Back_End_Project.Models.ManyToMany;

namespace Back_End_Project.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CategoryCourse> categoryCourses { get; set; }

    }
}
