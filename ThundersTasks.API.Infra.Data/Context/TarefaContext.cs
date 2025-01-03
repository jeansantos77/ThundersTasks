using Microsoft.EntityFrameworkCore;
using ThundersTasks.API.Domain.Entities;
using ThundersTasks.API.Infra.Data.EntityConfiguration;

namespace ThundersTasks.API.Infra.Data.Context
{
    public class TarefaContext: DbContext
    {
        public TarefaContext(DbContextOptions<TarefaContext> options) : base(options) { }

        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TarefaConfigurationMap());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            var currentTime = DateTime.Now;

            foreach (var entity in entities)
            {
                entity.Property("CreateDate").CurrentValue = currentTime;
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
