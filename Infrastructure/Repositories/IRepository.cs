using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface IRepository<TEntity> where TEntity : Base
    {
        Task<TEntity> ReadAsync(Guid id);
        Task<IEnumerable<TEntity>> ReadAllAsync();
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }

}
