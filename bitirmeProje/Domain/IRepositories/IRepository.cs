using bitirmeProje.Domain.Entities.Interface;
using System.Linq.Expressions;

namespace bitirmeProje.Domain.IRepositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> UpdateAsync(T entity);
        Task<T> CreateAsync(T entity);
        Task<List<T>> ListCreateAsync(List<T> entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(Guid Id);
        Task DeleteAsync(Expression<Func<T, bool>> predicate);
    }
}
