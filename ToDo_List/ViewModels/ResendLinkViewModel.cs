using System.ComponentModel.DataAnnotations;

namespace ToDo_List.PL.ViewModels
{
    public class ResendLinkViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}
