using System.ComponentModel.DataAnnotations;

namespace ToDo_List.PL.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "User Name Is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password And Confirm Password Must Be The Same")]
        public string ConfirmPassword { get; set; }
    }
}
