namespace Back_End_Project.Models
{
    public class SocialMedia
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
