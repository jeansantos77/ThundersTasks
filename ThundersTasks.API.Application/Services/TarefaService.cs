using AutoMapper;
using ThundersTasks.API.Application.Interfaces;
using ThundersTasks.API.Domain.Dtos;
using ThundersTasks.API.Domain.Entities;
using ThundersTasks.API.Domain.Interfaces;

namespace ThundersTasks.API.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private ITarefaRepository _tarefaRepository;
        private readonly IMapper _mapper;

        public TarefaService(ITarefaRepository tarefaRepository, IMapper mapper)
        {
            _tarefaRepository = tarefaRepository;
            _mapper = mapper;
        }

        public async Task<TarefaDto> Add(TarefaDto entity)
        {
            Tarefa mapEntity = _mapper.Map<Tarefa>(entity);
            mapEntity.Id = 0;

            Validation(mapEntity);

            Tarefa createdEntity = await _tarefaRepository.Add(mapEntity);

            return _mapper.Map<TarefaDto>(createdEntity);
        }

        private static void Validation(Tarefa tarefa)
        {
            if (tarefa.Conclusao > DateTime.UtcNow)
            {
                throw new Exception("A data de conclusão da tarefa NÃO pode ser superior a data atual.");
            }
        }

        public async Task<TarefaDto> ConcluirTarefa(int id)
        {
            Tarefa tarefa = await _tarefaRepository.GetByIdAsNoTracking(id) ?? throw new Exception($"Tarefa [ Id = {id}] não encontrada.");

            tarefa.Conclusao = DateTime.UtcNow;

            await _tarefaRepository.Update(tarefa);

            return _mapper.Map<TarefaDto>(tarefa); ;
        }

        public async Task Delete(int id)
        {
            Tarefa tarefa = await _tarefaRepository.GetByIdAsNoTracking(id) ?? throw new Exception($"Tarefa [ Id = {id}] não encontrada.");

            await _tarefaRepository.Delete(tarefa);
        }

        public async Task<List<TarefaDto>> GetAll()
        {
            return _mapper.Map<List<TarefaDto>>(await _tarefaRepository.GetAll(x => x.Id > 0));
        }

        public async Task<TarefaDto> GetById(int id)
        {
            return _mapper.Map<TarefaDto>(await _tarefaRepository.GetById(id));
        }

        public async Task Update(TarefaDto entity, int id)
        {
            Tarefa tarefa = await _tarefaRepository.GetByIdAsNoTracking(id) ?? throw new Exception($"Tarefa [ Id = {id}] não encontrada.");

            tarefa = _mapper.Map<Tarefa>(entity);
            tarefa.Id = id;

            Validation(tarefa);

            await _tarefaRepository.Update(tarefa);
        }
    }
}
