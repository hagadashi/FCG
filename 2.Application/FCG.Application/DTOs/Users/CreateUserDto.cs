using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs.Users
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$", ErrorMessage = "The password must be at least 8 characters long and include letters, numbers, and special characters.")]
        public string Password { get; set; } = string.Empty;

    }

}