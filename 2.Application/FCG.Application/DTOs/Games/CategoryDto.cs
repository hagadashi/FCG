
namespace FCG.Application.DTOs.Games
{

    public class CategoryDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int GameCount { get; set; }
    }

}