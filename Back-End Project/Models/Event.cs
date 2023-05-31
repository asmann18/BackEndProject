using Back_End_Project.Models.ManyToMany;

namespace Back_End_Project.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public ICollection<EventSpeaker> EventSpeakers { get; set; }
    }
}
