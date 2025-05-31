using FCG.Application.DTOs.Games;

namespace FCG.Application.Interfaces.Services.Games
{

    public interface ISaleService
    {
        Task<IEnumerable<SaleDto>> GetAllSalesAsync();
        Task<IEnumerable<SaleDto>> GetSalesByGameAsync(Guid gameId);
        Task<SaleDto?> GetSaleByIdAsync(Guid id);
        Task<SaleDto> CreateSaleAsync(CreateSaleDto createSaleDto);
        Task<SaleDto> UpdateSaleAsync(Guid id, UpdateSaleDto updateSaleDto);
        Task<bool> DeleteSaleAsync(Guid id);
    }

}