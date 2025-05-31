using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.DTOs.Games
{
    public class CreateSaleDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Discount percentage is required.")]
        [Range(0.01, 100.0, ErrorMessage = "Discount must be between 0.01% and 100%.")]
        public decimal DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Game ID is required.")]
        public Guid GameId { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public Guid CreatedByUserId { get; set; }
    }
}
