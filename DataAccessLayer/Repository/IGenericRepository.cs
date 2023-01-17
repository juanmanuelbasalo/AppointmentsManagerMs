using AppointmentsManagerMs.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace AppointmentsManagerMs.DataAccessLayer.Repository
{
    public interface IGenericRepository<TDbContext>
    {
        IQueryable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> searchTerm) where TEntity : class;
        Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> searchTerm) where TEntity : class;
        IQueryable<TEntity> GetAllReadOnly<TEntity>() where TEntity : class;
        IQueryable<TEntity> GetAllReadOnly<TEntity>(Expression<Func<TEntity, bool>> searchTerm) where TEntity : class;
        void Insert<TEntity>(TEntity entity) where TEntity : class;
        Task<bool> SaveAsync(string userEmail);
    }
}
