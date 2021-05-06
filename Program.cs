using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using StockData.Models;
using System.Threading;

namespace StockData
{
    public class Program
    {

        static volatile bool exit = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Press spacebar to cancel task.");

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var stockDataController = new StockDataController(cts);
            Task.Run(() =>
            {
                stockDataController.Run();
                while (!token.IsCancellationRequested)
                {
                    
                }
            }, token);

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Spacebar)
                    {
                        //set cancellation token to cancel
                        Console.WriteLine("Task Canceled.");
                        cts.Cancel();
                        break;
                    }
                }
            }
        }

    }
}