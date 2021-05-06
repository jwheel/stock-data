using MongoDB.Bson.Serialization.Attributes;
using System;


namespace StockData.Models
{
    public class MongoEntity
    {
        [BsonId]
        public Guid Id { get; set; }

        public MongoEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
