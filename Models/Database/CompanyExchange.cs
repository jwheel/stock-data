using System;
using System.Collections.Generic;
using System.Text;

namespace StockData.Models.Database
{
    /// <summary>
    /// A company and what exchange it's listed on. A company may list itself on multiple exchanges. 
    /// e.g. BP lists itself on the London Stock Exchange and the New York Stock Exchange
    /// </summary>
    public class CompanyExchange : MongoEntity
    {
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public IEnumerable<PricePoint> WeeklyAdjusted { get; set; }
        public IEnumerable<PricePoint> WeeklyUnadjusted { get; set; }
    }
}
