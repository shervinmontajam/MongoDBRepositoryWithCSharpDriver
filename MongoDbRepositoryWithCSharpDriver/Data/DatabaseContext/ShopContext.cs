using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbRepositoryWithCSharpDriver.Models.Settings;

namespace MongoDbRepositoryWithCSharpDriver.Data.DatabaseContext
{
    public class ShopContext
    {
        public readonly IMongoDatabase MongoDatabase;

        public ShopContext(IOptions<SettingModel> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            MongoDatabase = client.GetDatabase(settings.Value.Database);
        }
    }
}
