namespace Back_End_Project.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
         public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public int ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public byte StudentCount { get; set; }
        public string Assesments { get; set; }
    }
}
