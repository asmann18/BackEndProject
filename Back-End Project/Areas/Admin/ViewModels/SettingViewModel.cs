using System.ComponentModel.DataAnnotations;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class SettingViewModel
    {

        public int Id { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
