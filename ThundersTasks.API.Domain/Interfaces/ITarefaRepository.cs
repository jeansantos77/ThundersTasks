using ThundersTasks.API.Domain.Entities;

namespace ThundersTasks.API.Domain.Interfaces
{
    public interface ITarefaRepository : IBaseRepository<Tarefa>
    {
        Task<Tarefa> GetByIdAsNoTracking(int id);
    }
}
