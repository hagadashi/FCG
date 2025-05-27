using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs.Users
{
 
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "New password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Old password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string OldPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
    }

}
