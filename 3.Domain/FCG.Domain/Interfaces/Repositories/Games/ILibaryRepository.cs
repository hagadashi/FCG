using FCG.Domain.Entities.Games;

namespace FCG.Domain.Interfaces.Repositories.Games
{

    public interface ILibraryRepository : IBaseRepository<Library>
    {
        Task<IEnumerable<Library>> GetByUserIdAsync(Guid userId);
        Task<Library> GetByUserIdAndGameIdAsync(Guid userId, Guid gameId);
    }

}