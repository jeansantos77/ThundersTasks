using AutoMapper;
using Moq;
using ThundersTasks.API.Application.Services;
using ThundersTasks.API.Domain.Dtos;
using ThundersTasks.API.Domain.Entities;
using ThundersTasks.API.Domain.Interfaces;
using Xunit;

namespace ThundersTasks.API.Tests
{
    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _mockRepository = new Mock<ITarefaRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new TarefaService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnTarefaDto_WhenValidTarefa()
        {
            TarefaDto tarefaDto = getTarefaDto();
            var tarefa = getTarefa();

            _mockMapper.Setup(m => m.Map<Tarefa>(tarefaDto)).Returns(tarefa);
            _mockMapper.Setup(m => m.Map<TarefaDto>(It.IsAny<Tarefa>())).Returns(tarefaDto);
            _mockRepository.Setup(r => r.Add(It.IsAny<Tarefa>())).ReturnsAsync(tarefa);

            // Act
            var result = await _service.Add(tarefaDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tarefaDto.Titulo, result.Titulo);
            _mockRepository.Verify(r => r.Add(It.IsAny<Tarefa>()), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldThrowException_WhenConclusaoIsInFuture()
        {
            // Arrange
            var tarefaDto = getTarefaDto(1); ;
            var tarefa = getTarefa(1); ;

            _mockMapper.Setup(m => m.Map<Tarefa>(tarefaDto)).Returns(tarefa);
            _mockMapper.Setup(m => m.Map<TarefaDto>(It.IsAny<Tarefa>())).Returns(tarefaDto);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.Add(tarefaDto));
            Assert.Equal("A data de conclusão da tarefa NÃO pode ser superior a data atual.", exception.Message);
        }

        [Fact]
        public async Task ConcluirTarefa_ShouldReturnUpdatedTarefaDto_WhenValidId()
        {
            // Arrange
            int tarefaId = 1;
            var tarefaDto = getTarefaDto(); ;
            var tarefa = getTarefa(); ;


            _mockRepository.Setup(r => r.GetByIdAsNoTracking(tarefaId)).ReturnsAsync(tarefa);
            _mockRepository.Setup(r => r.Update(It.IsAny<Tarefa>())).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<TarefaDto>(It.IsAny<Tarefa>())).Returns(tarefaDto);

            // Act
            var result = await _service.ConcluirTarefa(tarefaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tarefaDto.Conclusao, result.Conclusao);
            _mockRepository.Verify(r => r.Update(It.IsAny<Tarefa>()), Times.Once);
        }

        [Fact]
        public async Task ConcluirTarefa_ShouldThrowException_WhenTarefaNotFound()
        {
            // Arrange
            int tarefaId = 999;

            _mockRepository.Setup(r => r.GetByIdAsNoTracking(tarefaId)).ReturnsAsync((Tarefa)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.ConcluirTarefa(tarefaId));
            Assert.Equal($"Tarefa [ Id = {tarefaId}] não encontrada.", exception.Message);
        }

        [Fact]
        public async Task Delete_ShouldRemoveTarefa_WhenValidId()
        {
            // Arrange
            int tarefaId = 1;
            var tarefa = getTarefa();

            _mockRepository.Setup(r => r.GetByIdAsNoTracking(tarefaId)).ReturnsAsync(tarefa);
            _mockRepository.Setup(r => r.Delete(It.IsAny<Tarefa>())).Returns(Task.CompletedTask);

            // Act
            await _service.Delete(tarefaId);

            // Assert
            _mockRepository.Verify(r => r.Delete(It.IsAny<Tarefa>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldThrowException_WhenTarefaNotFound()
        {
            // Arrange
            int tarefaId = 999;

            _mockRepository.Setup(r => r.GetByIdAsNoTracking(tarefaId)).ReturnsAsync((Tarefa)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.Delete(tarefaId));
            Assert.Equal($"Tarefa [ Id = {tarefaId}] não encontrada.", exception.Message);
        }

        [Fact]
        public async Task Update_ShouldUpdateTarefa_WhenValidData()
        {
            // Arrange
            int tarefaId = 1;
            var tarefaDto = getTarefaDto();
            var tarefa = getTarefa();

            _mockRepository.Setup(r => r.GetByIdAsNoTracking(tarefaId)).ReturnsAsync(tarefa);
            _mockMapper.Setup(m => m.Map<Tarefa>(tarefaDto)).Returns(getTarefa());
            _mockRepository.Setup(r => r.Update(It.IsAny<Tarefa>())).Returns(Task.CompletedTask);

            // Act
            await _service.Update(tarefaDto, tarefaId);

            // Assert
            _mockRepository.Verify(r => r.Update(It.IsAny<Tarefa>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldThrowException_WhenTarefaNotFound()
        {
            // Arrange
            int tarefaId = 999;
            var tarefaDto = getTarefaDto(1); ;

            _mockRepository.Setup(r => r.GetByIdAsNoTracking(tarefaId)).ReturnsAsync((Tarefa)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.Update(tarefaDto, tarefaId));
            Assert.Equal($"Tarefa [ Id = {tarefaId}] não encontrada.", exception.Message);
        }

        [Fact]
        public async Task GetById_ShouldReturnTarefaDto_WhenValidId()
        {
            // Arrange
            int tarefaId = 1;
            var tarefa = getTarefa();
            var tarefaDto = getTarefaDto();

            _mockRepository.Setup(r => r.GetById(tarefaId)).ReturnsAsync(tarefa);
            _mockMapper.Setup(m => m.Map<TarefaDto>(It.IsAny<Tarefa>())).Returns(tarefaDto);

            // Act
            var result = await _service.GetById(tarefaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tarefaDto.Titulo, result.Titulo);
        }

        private static TarefaDto getTarefaDto(int diasConclusao = 0)
        {
            // Arrange
            return new TarefaDto { Id = 1, Titulo = "Tarefa Teste", Prazo = DateTime.UtcNow.AddDays(1), Conclusao = DateTime.UtcNow.AddDays(diasConclusao) };
        }

        private static Tarefa getTarefa(int diasConclusao = 0)
        {
            // Arrange
            return new Tarefa { Id = 1, Titulo = "Tarefa Teste", Prazo = DateTime.UtcNow.AddDays(1), Conclusao = DateTime.UtcNow.AddDays(diasConclusao) };
        }

    }
}
