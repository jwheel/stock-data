using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using StockData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockData.Services
{
    public class MongoDbService
    {
        private string _connectionString;
        private string _dbString;
        private MongoClient _mongoClient;
        private IMongoDatabase _db;

        public MongoDbService(string connectionString, string database)
        {
            _connectionString = connectionString;
            _dbString = database;
            Initialize();
        }

        private void Initialize() {

            _mongoClient = new MongoClient(_connectionString);
            _db = _mongoClient.GetDatabase(_dbString);
        }

        public bool Put<T>(T item) where T : MongoEntity
        {
            
            BsonBinaryData binData = new BsonBinaryData(item.Id, GuidRepresentation.Standard);
            var collection = _db.GetCollection<T>(typeof(T).Name);
            var result = collection.ReplaceOne(new BsonDocument("_id", binData), item, new ReplaceOptions { IsUpsert = true });

            return result.IsAcknowledged;
        }

        public bool Put<T>(IEnumerable<T> items) where T : MongoEntity
        {            
            var collection = _db.GetCollection<T>(typeof(T).FullName);
            //collection.UpdateMany
            
            return true;
        }

        public List<T> Get<T>() where T : MongoEntity
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            return collection.Find(new BsonDocument()).ToList();
        }

    }
}
