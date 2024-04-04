using bitirmeProje.Domain.DataBaseContext;
using bitirmeProje.Domain.Entities.Interface;
using bitirmeProje.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace bitirmeProje.Domain.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly Context _context;
        public Repository(Context context)
        {
            _context = context;
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
          => _context.Set<T>().Where(predicate);

        public async virtual Task<T?> GetByIdAsync(Guid id)
            => await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);


        public async virtual Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().FirstOrDefaultAsync(predicate);

        public async virtual Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async virtual Task<T> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> ListCreateAsync(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async virtual Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async virtual Task DeleteAsync(Guid Id)
        {
            var data = await GetByIdAsync(Id);
            if (data is not null)
            {
                _context.Set<T>().Remove(data);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> tempEntityList = _context.Set<T>().Where(predicate);
            _context.Set<T>().RemoveRange(tempEntityList);
            await _context.SaveChangesAsync();
        } 

    }
}
