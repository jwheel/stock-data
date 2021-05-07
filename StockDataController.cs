using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using StockData.Models;
using System.Threading;

namespace StockData
{
    internal class StockDataController
    {

        public TickerFetchService tickerFetchService { get; set; }
        public CancellationTokenSource cancellationTokenSource { get; set; }
        
        
        public StockDataController(CancellationTokenSource cts)
        {
            cancellationTokenSource = cts;
        }

        public void Run()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .AddJsonFile("appsettings_secret.json")
              .Build();
            tickerFetchService = new TickerFetchService(configuration);
            tickerFetchService.Notified += OnTickerServiceBroadcast;            
            tickerFetchService.Start();
        }

        public static void OnTickerServiceBroadcast(object source, NotificationEventArgs e)
        {
            Console.WriteLine(e.Message);
        }     
    }
}
