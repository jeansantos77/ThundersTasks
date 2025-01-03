using System.Linq.Expressions;

namespace ThundersTasks.API.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<T> GetById(int id);
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);
    }
}
