using System;
using System.Collections.Generic;
using System.Text;

namespace StockData.Models
{
    public class NotificationEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
