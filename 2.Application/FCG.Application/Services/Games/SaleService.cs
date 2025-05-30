using AutoMapper;
using FCG.Application.DTOs.Games;
using FCG.Application.Interfaces.Services.Games;
using FCG.Domain.Interfaces.Repositories.Games;

namespace FCG.Application.Services.Games
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public SaleService(ISaleRepository saleRepository,
            IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SaleDto>> GetAllSalesAsync()
        {
            var sales = await _saleRepository.GetActiveAsync();
            var salesDto = _mapper.Map<IEnumerable<SaleDto>>(sales);

            return salesDto;
        }

        public async Task<SaleDto?> GetSaleByIdAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale is null) return null;
            var saleDto = _mapper.Map<SaleDto>(sale);
            return saleDto;
        }

        public async Task<IEnumerable<SaleDto>> GetSalesByGameAsync(Guid gameId)
        {
            var sales = await _saleRepository.GetByGameIdAsync(gameId);
            var salesDto = _mapper.Map<IEnumerable<SaleDto>>(sales);
            return salesDto;
        }
    }
}
