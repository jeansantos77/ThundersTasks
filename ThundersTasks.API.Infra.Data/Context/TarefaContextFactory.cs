using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ThundersTasks.API.Infra.Data.Context
{
    public class TarefaContextFactory : IDesignTimeDbContextFactory<TarefaContext>
    {
        public TarefaContext CreateDbContext(string[] args)
        {
            return CreateDbContext("DefaultConnection");
        }

        public TarefaContext CreateDbContext(string connectionStringName)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TarefaContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(connectionStringName));

            return new TarefaContext(optionsBuilder.Options);
        }
    }
}
