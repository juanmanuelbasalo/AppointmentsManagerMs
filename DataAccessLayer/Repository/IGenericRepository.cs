using AppointmentsManagerMs.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace AppointmentsManagerMs.DataAccessLayer.Repository
{
    public interface IGenericRepository
    {
        IQueryable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> searchTerm) where TEntity : BaseEntity;
        Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> searchTerm) where TEntity : BaseEntity;
        IQueryable<TEntity> GetAllReadOnly<TEntity>() where TEntity : BaseEntity;
        IQueryable<TEntity> GetAllReadOnly<TEntity>(Expression<Func<TEntity, bool>> searchTerm) where TEntity : BaseEntity;
        void Insert<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task<bool> SaveAsync(string userEmail);
    }
}
