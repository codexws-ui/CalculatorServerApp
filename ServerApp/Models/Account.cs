#nullable disable

using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "'{0}' field must contain between {2} and {1} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "'{1}' and '{0}' fields do not match")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginVM
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
