using AutoMapper;
using FCG.Application.DTOs.Games;
using FCG.Application.Interfaces.Services.Games;
using FCG.Domain.Entities.Games;
using FCG.Domain.Interfaces.Repositories.Games;
using FCG.Domain.Interfaces.Repositories.Users;

namespace FCG.Application.Services.Games
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SaleService(ISaleRepository saleRepository,
            IGameRepository gameRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _saleRepository = saleRepository;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
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

        public async Task<SaleDto> CreateSaleAsync(CreateSaleDto createSaleDto)
        {
            var game = await _gameRepository.GetByIdAsync(createSaleDto.GameId) ?? throw new KeyNotFoundException("Game not found");

            var user = await _userRepository.GetByIdAsync(createSaleDto.CreatedByUserId) ?? throw new KeyNotFoundException("User not found");

            var sale = new Sale(createSaleDto.Name, createSaleDto.Description, createSaleDto.DiscountPercentage, createSaleDto.StartDate, createSaleDto.EndDate, game, user);

            var createdSale = await _saleRepository.AddAsync(sale);
            await _saleRepository.SaveChangesAsync();

            return _mapper.Map<SaleDto>(createdSale);
        }

        public async Task<SaleDto> UpdateSaleAsync(Guid id, UpdateSaleDto updateSaleDto)
        {
            var sale = await _saleRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Sale not found");

            var _game = sale.Game;
            if (updateSaleDto.GameId.HasValue)
            {
                var game = await _gameRepository.GetByIdAsync(updateSaleDto.GameId.Value) ?? throw new KeyNotFoundException("Game not found");
                _game = game;
            }

            sale.Update(updateSaleDto.Name, updateSaleDto.Description, updateSaleDto.DiscountPercentage, updateSaleDto.StartDate, updateSaleDto.EndDate, _game);

            if (updateSaleDto.IsActive.HasValue)
            {
                if (updateSaleDto.IsActive.Value)
                    sale.Activate();
                else
                    sale.Deactivate();
            }

            var updatedSale = await _saleRepository.UpdateAsync(sale);
            await _saleRepository.SaveChangesAsync();

            return _mapper.Map<SaleDto>(updatedSale);
        }

        public async Task<bool> DeleteSaleAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale is null) return false;
            await _saleRepository.DeleteAsync(sale);
            await _saleRepository.SaveChangesAsync();
            return true;
        }
    }
}
