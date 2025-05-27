
using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs.Users
{

    public class ChangeUserRoleDto
    {
        [Required(ErrorMessage = "New Role ID is required.")]
        public Guid NewRoleId { get; set; }
    }

}
