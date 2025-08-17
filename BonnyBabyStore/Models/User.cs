using System.ComponentModel.DataAnnotations;

namespace BonnyBabyStore.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 15 characters")]
        public string? UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? PasswordHash { get; set; }
        [Display(Name = "First Name")]
        [StringLength(50)]
        [Required]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string? LastName { get; set; }
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int Age { get; set; }
        public string? Gender { get; set; }
        [Display(Name = "Photo")]
        public string? ImageUrl { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
