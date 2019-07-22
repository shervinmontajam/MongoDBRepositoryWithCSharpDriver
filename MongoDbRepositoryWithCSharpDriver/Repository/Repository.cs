using MongoDB.Driver;
using MongoDbRepositoryWithCSharpDriver.Data.DatabaseContext;
using MongoDbRepositoryWithCSharpDriver.Data.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace MongoDbRepositoryWithCSharpDriver.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected IMongoCollection<TEntity> Collection { get; }

        protected ShopContext ShopContext;

        internal Repository(ShopContext shopContext)
        {
            ShopContext = shopContext;
            Collection = ShopContext.MongoDatabase.GetCollection<TEntity>(typeof(TEntity).GetCustomAttribute<MongoCollectionAttribute>().CollectionName);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await Collection.Find(filter => true).ToListAsync();
        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression) => await Collection.FindSync(expression).FirstOrDefaultAsync();
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression) => await Collection.FindSync(expression).ToListAsync();

        public async Task AddAsync(TEntity item) => await Collection.InsertOneAsync(item);
        public async Task AddRangeAsync(IEnumerable<TEntity> items) => await Collection.InsertManyAsync(items);

        public async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity item)
        {
            var updateResult = await Collection.ReplaceOneAsync(expression, item);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
        public async Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> expression, IEnumerable<TEntity> items)
        {
            var effected = 0;
            foreach (var item in items)
            {
                var updateResult = await Collection.ReplaceOneAsync(expression, item);
                effected += updateResult.IsAcknowledged && updateResult.ModifiedCount > 0 ? 1 : 0;
            }

            return effected;
        }

        public async Task<bool> Remove(Expression<Func<TEntity, bool>> expression)
        {
            var deleteResult = await Collection.DeleteOneAsync(expression);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        public async Task<long> RemoveRange(Expression<Func<TEntity, bool>> expression)
        {
            var deleteResult = await Collection.DeleteManyAsync(expression);
            return deleteResult.IsAcknowledged ? deleteResult.DeletedCount : 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

