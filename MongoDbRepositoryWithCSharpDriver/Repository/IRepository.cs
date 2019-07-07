using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDbRepositoryWithCSharpDriver.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression);


        Task AddAsync(TEntity item);
        Task AddRangeAsync(IEnumerable<TEntity> items);

        Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity item);
        Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> expression, IEnumerable<TEntity> items);

        Task<bool> Remove(Expression<Func<TEntity, bool>> expression);
        Task<long> RemoveRange(Expression<Func<TEntity, bool>> expression);

    }
}