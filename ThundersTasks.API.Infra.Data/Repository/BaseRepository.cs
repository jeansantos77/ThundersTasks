using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ThundersTasks.API.Domain.Interfaces;
using ThundersTasks.API.Infra.Data.Context;

namespace ThundersTasks.API.Infra.Data.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly TarefaContext _tarefaContext;
        public BaseRepository(TarefaContext context)
        {
            _tarefaContext = context;
        }

        public async Task<T> Add(T entity)
        {
            await _tarefaContext.Set<T>().AddAsync(entity);
            await _tarefaContext.SaveChangesAsync();

            return entity;

        }

        public async Task Delete(T entity)
        {
            _tarefaContext.Set<T>().Remove(entity);
            await _tarefaContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            return await _tarefaContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _tarefaContext.Set<T>().FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _tarefaContext.Set<T>().Update(entity);
            await _tarefaContext.SaveChangesAsync();
        }
    }
}
