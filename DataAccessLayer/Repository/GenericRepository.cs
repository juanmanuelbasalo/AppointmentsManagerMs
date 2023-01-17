using AppointmentsManagerMs.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppointmentsManagerMs.DataAccessLayer.Repository
{
    public class GenericRepository<TDbContext> : IGenericRepository<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        public GenericRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAllReadOnly<TEntity>() where TEntity : class
        {
            var entities = _dbContext.Set<TEntity>().AsNoTracking();
            return entities;
        }
        public IQueryable<TEntity> GetAllReadOnly<TEntity>(Expression<Func<TEntity, bool>> searchTerm) where TEntity : class
        {
            var entities = _dbContext.Set<TEntity>().Where(searchTerm).AsNoTracking();
            return entities;
        }
        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            var entities = _dbContext.Set<TEntity>();
            entities.Add(entity);
        }
        public async Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> searchTerm) where TEntity : class
        {
            var entities = _dbContext.Set<TEntity>();
            var entity = await entities.FirstOrDefaultAsync(searchTerm);

            return entity;
        }
        public IQueryable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> searchTerm) where TEntity : class
        {
            var entities = _dbContext.Set<TEntity>();
            var allEntities = entities.Where(searchTerm);

            return allEntities;
        }
        public async Task<bool> SaveAsync(string userEmail)
        {
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

    }

    public static class RepositoryExtensions
    {
        public static IQueryable<TEntity> IncludeRelatedEntities<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            foreach (var include in includes)
            {
                if (include.Body is MemberExpression)
                    query = query.Include(include);
            }

            return query;
        }
    }
}
