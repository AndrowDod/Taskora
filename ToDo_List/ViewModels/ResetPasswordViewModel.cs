using System.ComponentModel.DataAnnotations;

namespace ToDo_List.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; }

        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [Display(Name ="New Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password And Confirm Password Must Be The Same")]
        public string ConfirmPassword { get; set; }

    }
}
