namespace ThundersTasks.API.Application.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<T> Add(T entity);
        Task Update(T entity, int id);
        Task Delete(int id);
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
    }
}
