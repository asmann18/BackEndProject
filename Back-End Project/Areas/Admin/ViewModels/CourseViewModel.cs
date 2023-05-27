namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Desc { get; set; }
        public int Price { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public int ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public byte StudentCount { get; set; }
        public string Assesments { get; set; }
    }
}
