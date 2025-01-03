using Microsoft.EntityFrameworkCore;
using ThundersTasks.API.Application.Interfaces;
using ThundersTasks.API.Application.Services;
using ThundersTasks.API.Domain.Interfaces;
using ThundersTasks.API.Infra.Data.Context;
using ThundersTasks.API.Infra.Data.Repository;
using ThundersTasks.API.Middleware;
using ThundersTasks.API.Application.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var Configuration = builder.Configuration;

builder.Services.AddDbContext<TarefaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<TarefaContext>();
    
//    var pendingMigrations = dbContext.Database.GetPendingMigrations();
//    if (pendingMigrations.Any())
//    {
//        dbContext.Database.Migrate();
//    }
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
