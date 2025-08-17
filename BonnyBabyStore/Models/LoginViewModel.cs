using System.ComponentModel.DataAnnotations;

namespace BonnyBabyStore.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email or username required")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}


