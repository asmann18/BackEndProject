namespace Back_End_Project.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Redactor { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }

        public Blog()
        {
            CreatedTime = DateTime.UtcNow;
        }
    }
}
