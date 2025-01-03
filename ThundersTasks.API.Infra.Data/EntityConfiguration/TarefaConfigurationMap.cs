using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThundersTasks.API.Domain.Entities;

namespace ThundersTasks.API.Infra.Data.EntityConfiguration
{
    public class TarefaConfigurationMap : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Titulo)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(t => t.Descricao)
                   .HasMaxLength(1000);
        }
    }
}
