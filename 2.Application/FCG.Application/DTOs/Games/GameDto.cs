
namespace FCG.Application.DTOs.Games
{

    public class GameDto : BaseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public decimal? SalePrice { get; set; }
        public bool IsOnSale { get; set; }
    }

}