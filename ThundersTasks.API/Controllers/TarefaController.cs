using Microsoft.AspNetCore.Mvc;
using ThundersTasks.API.Application.Interfaces;
using ThundersTasks.API.Domain.Dtos;

namespace ThundersTasks.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ILogger<TarefaController> _logger;
        private readonly ITarefaService _tarefaService;

        public TarefaController(ILogger<TarefaController> logger, ITarefaService tarefaService)
        {
            _logger = logger;
            _tarefaService = tarefaService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] TarefaDto tarefa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            TarefaDto taskCreated;

            try
            {
                taskCreated = await _tarefaService.Add(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao adicionar tarefa: { ex.InnerException?.Message ?? ex.Message }");
            }

            return StatusCode(StatusCodes.Status201Created, taskCreated);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] TarefaDto tarefa, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _tarefaService.Update(tarefa, id);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar tarefa: { ex.InnerException?.Message ?? ex.Message }");
            }

            return Ok(tarefa);
        }

        [HttpPut("ConcluirTarefa/{id}")]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConcluirTarefa(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            TarefaDto tarefa;

            try
            {
                tarefa = await _tarefaService.ConcluirTarefa(id);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao concluir a tarefa: {ex.InnerException?.Message ?? ex.Message}");
            }

            return Ok(tarefa);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _tarefaService.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar tarefa: { ex.InnerException?.Message ?? ex.Message }");
            }

            return Ok();
        }

        [HttpGet("All")]
        [ProducesResponseType(typeof(List<TarefaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tarefaService.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _tarefaService.GetById(id));
        }

    }
}
