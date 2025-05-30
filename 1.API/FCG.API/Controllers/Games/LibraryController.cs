using FCG.Application.Interfaces.Services.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Controllers.Games
{
    [Authorize]
    public class LibraryController : BaseController
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        /// <summary>
        /// Obtém bibliotecas por usuário
        /// </summary>
        /// <param name="userId">ID do jogo</param>
        [HttpGet("by-user/{userId:guid}")]
        public async Task<IActionResult> GetLibrariesByUser(Guid userId)
        {
            var libs = await _libraryService.GetLibrariesByUserAsync(userId);
            return HandleResult(libs);
        }
    }
}
