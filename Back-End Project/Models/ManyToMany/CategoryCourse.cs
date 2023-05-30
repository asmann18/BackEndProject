namespace Back_End_Project.Models.ManyToMany
{
    public class CategoryCourse
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category category { get; set; }
        public int CourseId { get; set; }
        public Course course { get; set; }
    }
}
