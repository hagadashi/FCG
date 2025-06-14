﻿using FCG.Domain.Entities.Games;


namespace FCG.Domain.Interfaces.Repositories.Games
{

    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);
        Task<Category> GetByIdWithGamesAsync(Guid id);
    }

}