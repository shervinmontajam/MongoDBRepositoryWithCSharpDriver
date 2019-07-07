using MongoDbRepositoryWithCSharpDriver.Data.DatabaseContext;
using MongoDbRepositoryWithCSharpDriver.Data.Entities;
using MongoDB.Driver;

namespace MongoDbRepositoryWithCSharpDriver.Repository
{
    public class ProductRepository : Repository<ProductEntity>, IProductRepository
    {
        private readonly ShopContext _shopContext;

        public ProductRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        protected override IMongoCollection<ProductEntity> Collection => _shopContext.MongoDatabase.GetCollection<ProductEntity>("products");

    }
}
