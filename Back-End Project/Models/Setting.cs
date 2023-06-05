using System.ComponentModel.DataAnnotations;

namespace Back_End_Project.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
