using FCG.Application.DTOs.Games;

namespace FCG.Application.Interfaces.Services.Games
{

    public interface ILibraryService
    {
        Task<IEnumerable<LibraryDto>> GetLibrariesByUserAsync(Guid userId);
    }

}