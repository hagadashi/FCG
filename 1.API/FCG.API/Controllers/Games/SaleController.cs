using FCG.Application.DTOs.Games;
using FCG.Application.Interfaces.Services.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Controllers.Games
{
    [Authorize]
    public class SaleController : BaseController
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
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
            var sales = await _saleService.GetSalesByGameAsync(gameId);
            return HandleResult(sales);
        }

        /// <summary>
        /// Cria uma nova promoção
        /// </summary>
        /// <param name="createSaleDto">Dados da promoção</param>
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreatSale([FromBody] CreateSaleDto createSaleDto)
        {
            var sale = await _saleService.CreateSaleAsync(createSaleDto);
            return CreatedAtAction(nameof(GetSaleById), new { id = sale.Id }, sale);
        }

        /// <summary>
        /// Atualiza uma promoção existente
        /// </summary>
        /// <param name="id">ID da promoção</param>
        /// <param name="updateSaleDto">Dados atualizados da promoção</param>
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateGame(Guid id, [FromBody] UpdateSaleDto updateSaleDto)
        {
            var sale = await _saleService.UpdateSaleAsync(id, updateSaleDto);
            return HandleResult(sale, "Promotion updated successfully!");
        }

        /// <summary>
        /// Remove uma promoção
        /// </summary>
        /// <param name="id">ID da promoção</param>
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteSale(Guid id)
        {
            var success = await _saleService.DeleteSaleAsync(id);

            if (!success)
                return NotFound("Promotion not found.");

            return NoContent();
        }
    }
}
