using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using StockData.Models;
using StockData.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Timers;

namespace StockData
{
    public class TickerFetchService
    {
        private readonly IConfiguration Configuration;
        public ConfigurationItems ConfigurationItems;
        
        public delegate void TickerFetchNotifyEventHandler(object source, NotificationEventArgs args);
        public event TickerFetchNotifyEventHandler Notified;
        private List<string> _nasdaqSymbolList { get; set; }
        

        private System.Timers.Timer _fetchTimer;
        private Metadata _metadata;
        private MongoDbService _mongoService;

        public TickerFetchService(IConfiguration configuration)
        {
            Configuration = configuration;
            Initialize();
        }

        private  void Initialize()
        {
            var configurationItems = new ConfigurationItems();
            Configuration.GetSection("ConfigurationItems").Bind(configurationItems);
            ConfigurationItems = configurationItems;
            _nasdaqSymbolList = GetNasdaqSymbolList().ToList();
            _mongoService = new MongoDbService(ConfigurationItems.MongoConnectionString, "stock-options");
            var metadata = _mongoService.Get<Metadata>();
            if (metadata.Count == 0)
            {
                _mongoService.Put<Metadata>(new Metadata() { DailyAttempt = 0, StockIndex = 0 });
                metadata = _mongoService.Get<Metadata>();
            }
            _metadata = metadata.FirstOrDefault();

        }

        protected virtual void OnNotify(string value)
        {
            if (Notified != null)
            {
                var message = new NotificationEventArgs();
                message.Message = value;
                Notified(this, message);
            }
        }

        public void Start()
        {
            _fetchTimer = new System.Timers.Timer(2000);
            _fetchTimer.Elapsed += FetchEvent;
            _fetchTimer.AutoReset = true;
            _fetchTimer.Enabled = true;
        }

        private IEnumerable<String> GetNasdaqSymbolList()
        {
            var jsonString = File.ReadAllText("data/nasdaq-listed_json.json");
            var companies = JsonSerializer.Deserialize<List<NasdaqItem>>(jsonString);
            var symbols = companies.Select(x => x.Symbol);
            return symbols;
        }

        private void FetchEvent(Object source, ElapsedEventArgs e)
        {
            OnNotify(_nasdaqSymbolList[_metadata.StockIndex++]);
            _mongoService.Put(_metadata);
            
        }
    }
}
