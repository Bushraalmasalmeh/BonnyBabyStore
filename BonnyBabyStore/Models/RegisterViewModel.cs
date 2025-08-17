using System.ComponentModel.DataAnnotations;

namespace BonnyBabyStore.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "incorrect Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password ")]
        public string ConfirmPassword { get; set; }
    }
}


