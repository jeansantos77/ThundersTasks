namespace ThundersTasks.API.Domain.Dtos
{
    public class TarefaDto
    {
        public required string Titulo { get; set; }
        public string? Descricao { get; set; }
        public required DateTime Prazo { get; set; }
        public DateTime? Conclusao { get; set; }
    }
}
