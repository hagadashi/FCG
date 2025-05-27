using FCG.Domain.Entities;

namespace FCG.Application.Interfaces
{

    public interface IBaseService<TEntity, TDto> where TEntity : Entity
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(Guid id);
        Task<TDto> CreateAsync(TDto dto);
        Task<TDto> UpdateAsync(Guid id, TDto dto);
        Task<bool> DeleteAsync(Guid id);
    }

}