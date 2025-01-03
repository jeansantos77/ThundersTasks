using Microsoft.EntityFrameworkCore;
using ThundersTasks.API.Domain.Entities;
using ThundersTasks.API.Domain.Interfaces;
using ThundersTasks.API.Infra.Data.Context;

namespace ThundersTasks.API.Infra.Data.Repository
{
    public class TarefaRepository : BaseRepository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(TarefaContext context) : base(context) {}

        public async Task<Tarefa> GetByIdAsNoTracking(int id)
        {
            return await _tarefaContext.Set<Tarefa>().Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
