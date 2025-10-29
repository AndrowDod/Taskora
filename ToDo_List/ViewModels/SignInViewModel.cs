using System.ComponentModel.DataAnnotations;

namespace ToDo_List.PL.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }


    }
}
