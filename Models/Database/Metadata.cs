
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace StockData.Models
{
    public class Metadata : MongoEntity
    {
        
        public int StockIndex { get; set; }
        public int DailyAttempt { get; set; }
        
        public Metadata()
        {

        }
    }
}
