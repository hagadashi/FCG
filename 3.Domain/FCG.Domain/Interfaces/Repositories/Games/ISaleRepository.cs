﻿using FCG.Domain.Entities.Games;

namespace FCG.Domain.Interfaces.Repositories.Games
{

    public interface ISaleRepository : IBaseRepository<Sale>
    {
        Task<IEnumerable<Sale>> GetActiveAsync();
        Task<IEnumerable<Sale>> GetByGameIdAsync(Guid gameId);
        Task<IEnumerable<Sale>> GetActiveByGameIdAsync(Guid gameId);
    }

}