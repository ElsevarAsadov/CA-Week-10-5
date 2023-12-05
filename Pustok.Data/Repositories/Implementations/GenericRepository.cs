using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Pustok.Data.Repositories.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
                    where TEntity : BaseEntity, new()
    {
        private readonly PustokContext _context;

        public GenericRepository(PustokContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>(); // _context.Books

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            Table.Remove(entity);
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? expression = null, params string[]? includes) //{"Authors","BookImages", "BookTags.Tag"}
        {
            var query = GetQuery(includes);

            return expression is not null
                        ? await query.Where(expression).ToListAsync()
                        : await query.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>>? expression = null, params string[]? includes)
        {
            var query = GetQuery(includes);

            return expression is not null
                    ? await query.Where(expression).FirstOrDefaultAsync()
                    : await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            Table.Update(entity);
        }

        private IQueryable<TEntity> GetQuery(string[] includes)
        {
            var query = Table.AsQueryable();
            if (includes is not null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            return query;
        }
    }
}
