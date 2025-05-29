using Microsoft.AspNetCore.Mvc;
using FCG.Application.DTOs.Games;
using FCG.Application.Interfaces.Services.Games;

namespace FCG.API.Controllers.Games;

public class GamesController : BaseController
{

    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Obtém todos os jogos
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await _gameService.GetAllGamesAsync();
        return HandleResult(games);
    }

    /// <summary>
    /// Obtém um jogo por ID
    /// </summary>
    /// <param name="id">ID do jogo</param>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetGameById(Guid id)
    {
        var game = await _gameService.GetGameByIdAsync(id);
        return HandleResult(game);
    }

    /// <summary>
    /// Obtém jogos por categoria
    /// </summary>
    /// <param name="categoryId">ID da categoria</param>
    [HttpGet("by-category/{categoryId:guid}")]
    public async Task<IActionResult> GetGamesByCategory(Guid categoryId)
    {
        var games = await _gameService.GetGamesByCategoryAsync(categoryId);
        return HandleResult(games);
    }


    /// <summary>
    /// Cria um novo jogo
    /// </summary>
    /// <param name="createGameDto">Dados do jogo</param>
    [HttpPost]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameDto createGameDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var game = await _gameService.CreateGameAsync(createGameDto);
        return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
    }

    /// <summary>
    /// Atualiza um jogo existente
    /// </summary>
    /// <param name="id">ID do jogo</param>
    /// <param name="updateGameDto">Dados atualizados do jogo</param>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateGame(Guid id, [FromBody] UpdateGameDto updateGameDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var game = await _gameService.UpdateGameAsync(id, updateGameDto);
        return HandleResult(game, "Jogo atualizado com sucesso");
    }

    /// <summary>
    /// Remove um jogo
    /// </summary>
    /// <param name="id">ID do jogo</param>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        var success = await _gameService.DeleteGameAsync(id);

        if (!success)
            return NotFound("Jogo não encontrado");

        return NoContent();
    }

}