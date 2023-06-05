using System.ComponentModel.DataAnnotations;

namespace Back_End_Project.Areas.Admin.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
