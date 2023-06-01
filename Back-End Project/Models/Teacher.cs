using System.ComponentModel.DataAnnotations;

namespace Back_End_Project.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Profession { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public int Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
        [Phone]
        public int Phone { get; set; }


        public List<Skill> Skills { get; set; }
        public List<SocialMedia> SocialMedias { get; set; }
        
    }
}
