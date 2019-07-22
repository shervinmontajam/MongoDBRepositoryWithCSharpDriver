using MongoDbRepositoryWithCSharpDriver.Data.DatabaseContext;
using MongoDbRepositoryWithCSharpDriver.Data.Entities;
using MongoDB.Driver;

namespace MongoDbRepositoryWithCSharpDriver.Repository
{
    public class ProductRepository : Repository<ProductEntity>, IProductRepository
    {
        public ProductRepository(ShopContext shopContext): base (shopContext)
        {

        }

    }
}
