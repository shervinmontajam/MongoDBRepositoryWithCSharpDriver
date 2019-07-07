﻿using MongoDbRepositoryWithCSharpDriver.Data.Entities;

namespace MongoDbRepositoryWithCSharpDriver.Models.ApiRequests
{
    public class CreateProductRequestModel
    {
        public string Name { get; set; }

        public ImageEntity Image { get; set; }

        public decimal Price { get; set; }
    }
}
