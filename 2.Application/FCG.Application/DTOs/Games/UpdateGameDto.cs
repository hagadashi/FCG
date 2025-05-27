using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs.Games
{

    public class UpdateGameDto
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public Guid? CategoryId { get; set; }

        public bool? IsActive { get; set; }
    }

}