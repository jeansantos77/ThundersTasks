namespace ThundersTasks.API.Domain.Entities
{
    public class Tarefa: Base
    {
        public required string Titulo { get; set; }
        public string? Descricao { get; set; }
        public required DateTime Prazo { get; set; }
        public DateTime? Conclusao { get; set; }
    }
}
