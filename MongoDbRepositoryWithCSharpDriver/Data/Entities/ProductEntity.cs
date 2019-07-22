using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDbRepositoryWithCSharpDriver.Data.Metadata;

namespace MongoDbRepositoryWithCSharpDriver.Data.Entities
{
    [MongoCollection("products")]
    public class ProductEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public Guid GuidId { get; set; }

        public string Name { get; set; }

        [BsonDateTimeOptions]
        public DateTime LastUpdate { get; set; }

        public ImageEntity Image { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; } = 0;
    }
}
