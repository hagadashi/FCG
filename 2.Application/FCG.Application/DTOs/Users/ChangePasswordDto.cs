using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs.Users
{
 
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$", ErrorMessage = "The password must be at least 8 characters long and include letters, numbers, and special characters.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Old password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string OldPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
    }

}
