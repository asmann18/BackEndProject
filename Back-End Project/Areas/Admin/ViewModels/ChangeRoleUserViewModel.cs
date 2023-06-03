using Microsoft.Build.Framework;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class ChangeRoleUserViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
