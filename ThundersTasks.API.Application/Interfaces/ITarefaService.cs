using ThundersTasks.API.Domain.Dtos;

namespace ThundersTasks.API.Application.Interfaces
{
    public interface ITarefaService : IBaseService<TarefaDto>
    {
        Task<TarefaDto> ConcluirTarefa(int id);
    }
}
