using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ThundersTasks.API.Application.Interfaces;
using ThundersTasks.API.Controllers;
using ThundersTasks.API.Domain.Dtos;
using Xunit;

namespace ThundersTasks.API.Tests
{
    public class TarefaControllerTests
    {
        private readonly Mock<ITarefaService> _mockTarefaService;
        private readonly Mock<ILogger<TarefaController>> _mockLogger;
        private readonly TarefaController _controller;

        public TarefaControllerTests()
        {
            // Criar mock para o serviço
            _mockTarefaService = new Mock<ITarefaService>();
            _mockLogger = new Mock<ILogger<TarefaController>>();
            _controller = new TarefaController(_mockLogger.Object, _mockTarefaService.Object);
        }

        [Fact]
        public async Task Add_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Tarefa", "Campo obrigatório");

            // Act
            var result = await _controller.Add(new TarefaDto { Titulo = "Título", Prazo = DateTime.Today });

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Add_ReturnsBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            var tarefa = new TarefaDto { Titulo = "Título", Prazo = DateTime.Today };
            _mockTarefaService.Setup(service => service.Add(It.IsAny<TarefaDto>()))
                              .ThrowsAsync(new Exception("Erro desconhecido"));

            // Act
            var result = await _controller.Add(tarefa);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro desconhecido", badRequestObjectResult.Value?.ToString());
        }

        [Fact]
        public async Task Add_ReturnsCreatedAtAction_WhenTarefaIsSuccessfullyAdded()
        {
            // Arrange
            var tarefa = new TarefaDto { Titulo = "Título", Prazo = DateTime.Today };
            var createdTask = new TarefaDto { Titulo = "Título", Prazo = DateTime.Today };
            _mockTarefaService.Setup(service => service.Add(It.IsAny<TarefaDto>()))
                              .ReturnsAsync(createdTask);

            // Act
            var result = await _controller.Add(tarefa);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
            Assert.Equal(createdTask, createdAtActionResult.Value);
        }

        [Fact]
        public async Task ConcluirTarefa_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("id", "Campo obrigatório");

            // Act
            var result = await _controller.ConcluirTarefa(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ConcluirTarefa_ReturnsBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            _mockTarefaService.Setup(service => service.ConcluirTarefa(It.IsAny<int>()))
                              .ThrowsAsync(new Exception("Erro desconhecido"));

            // Act
            var result = await _controller.ConcluirTarefa(1);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro desconhecido", badRequestObjectResult.Value?.ToString());
        }

        [Fact]
        public async Task ConcluirTarefa_ReturnsOk_WhenTarefaIsSuccessfullyConcluded()
        {
            // Arrange
            var tarefaConcluida = new TarefaDto { Id = 1, Titulo = "Tarefa Concluída", Prazo = DateTime.Today, Conclusao = DateTime.Today };

            _mockTarefaService.Setup(service => service.ConcluirTarefa(It.IsAny<int>()))
                              .ReturnsAsync(tarefaConcluida);

            // Act
            var result = await _controller.ConcluirTarefa(tarefaConcluida.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(tarefaConcluida, okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            var tarefaToUpdate = new TarefaDto { Titulo = "Título", Prazo = DateTime.Today };

            _mockTarefaService.Setup(service => service.Update(It.IsAny<TarefaDto>(), It.IsAny<int>()))
                              .ThrowsAsync(new Exception("Erro desconhecido"));

            // Act
            var result = await _controller.Update(tarefaToUpdate, 1);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro desconhecido", badRequestObjectResult.Value?.ToString());
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenTarefaDoesNotExist()
        {
            // Arrange
            var tarefaToUpdate = new TarefaDto { Titulo = "Título", Prazo = DateTime.Today };

            _mockTarefaService.Setup(service => service.Update(It.IsAny<TarefaDto>(), It.IsAny<int>()))
                              .ThrowsAsync(new Exception("Tarefa [ Id = 1 ] não encontrada."));

            // Act
            var result = await _controller.Update(tarefaToUpdate, 1);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro ao atualizar tarefa: Tarefa [ Id = 1 ] não encontrada.", badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenTarefaIsSuccessfullyUpdated()
        {
            // Arrange
            var tarefaToUpdate = new TarefaDto { Titulo = "Título", Prazo = DateTime.Today };

            _mockTarefaService.Setup(service => service.Update(It.IsAny<TarefaDto>(), It.IsAny<int>()))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(tarefaToUpdate, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(tarefaToUpdate, okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            _mockTarefaService.Setup(service => service.Delete(It.IsAny<int>()))
                              .ThrowsAsync(new Exception("Erro ao deletar tarefa"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao deletar tarefa", badRequestObjectResult.Value?.ToString());
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenTarefaDoesNotExist()
        {
            // Arrange
            _mockTarefaService.Setup(service => service.Delete(It.IsAny<int>()))
                              .ThrowsAsync(new Exception("Tarefa [ Id = 1 ] não encontrada."));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro ao deletar tarefa: Tarefa [ Id = 1 ] não encontrada.", badRequestObjectResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenTarefaIsSuccessfullyDeleted()
        {
            // Arrange
            _mockTarefaService.Setup(service => service.Delete(It.IsAny<int>()))
                              .Returns(Task.CompletedTask);  // Simula a execução do método sem retornar nada

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);  // OkResult é retornado quando a exclusão é bem-sucedida
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenTarefasExist()
        {
            // Arrange
            var tarefas = new List<TarefaDto>
            {
                new TarefaDto { Id = 1, Titulo = "Tarefa 1", Prazo = DateTime.Today },
                new TarefaDto { Id = 2, Titulo = "Tarefa 2", Prazo = DateTime.Today.AddDays(2)}
            };

            // Mockando o serviço para retornar a lista de tarefas
            _mockTarefaService.Setup(service => service.GetAll())
                              .ReturnsAsync(tarefas);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnValue = Assert.IsType<List<TarefaDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);  // Verifica se retornou as 2 tarefas
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenTarefaIsFound()
        {
            // Arrange
            var tarefaId = 1;
            var tarefa = new TarefaDto { Id = tarefaId, Titulo = "Tarefa 1", Prazo = DateTime.Today };

            // Mockando o serviço para retornar a tarefa com id 1
            _mockTarefaService.Setup(service => service.GetById(tarefaId))
                              .ReturnsAsync(tarefa);

            // Act
            var result = await _controller.GetById(tarefaId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Verifica se é um OkObjectResult
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);  // Verifica se o status é 200 OK
            var returnValue = Assert.IsType<TarefaDto>(okResult.Value);  // Verifica se o valor retornado é do tipo TarefaDto
            Assert.Equal(tarefaId, returnValue.Id);  // Verifica se o ID da tarefa está correto
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTarefaIsNotFound()
        {
            // Arrange
            var tarefaId = 1;

            // Mockando o serviço para retornar null, simulando que a tarefa não foi encontrada
            _mockTarefaService.Setup(service => service.GetById(tarefaId))
                              .ReturnsAsync((TarefaDto)null);

            // Act
            var result = await _controller.GetById(tarefaId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);  // Verifica se é um NotFoundResult
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);  // Verifica se o status é 404 Not Found
        }

        

    }
}
