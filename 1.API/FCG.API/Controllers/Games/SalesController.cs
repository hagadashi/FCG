using FCG.Application.Interfaces.Services.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Controllers.Games
{
    [Authorize]
    public class SalesController : BaseController
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        /// <summary>
        /// Obtém todas as promoções
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return HandleResult(sales);
        }

        /// <summary>
        /// Obtém uma promoção por ID
        /// </summary>
        /// <param name="id">ID do jogo</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            var sale = await _saleService.GetSaleByIdAsync(id);
            return HandleResult(sale);
        }

        /// <summary>
        /// Obtém promoções por jogo
        /// </summary>
        /// <param name="gameId">ID do jogo</param>
        [HttpGet("by-game/{gameId:guid}")]
        public async Task<IActionResult> GetSalesByGame(Guid gameId)
        {
            var games = await _saleService.GetSalesByGameAsync(gameId);
            return HandleResult(games);
        }
    }
}
