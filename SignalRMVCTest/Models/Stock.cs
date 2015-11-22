using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMVCTest.Models
{
    public class Stock
    {
        private decimal _price;
        public string Symbol { get; set; }

        public decimal Price
        {
            get { return _price; }
            set
            {
                if (_price == value)
                {
                    return;
                }

                _price = value;
                if (DayOpen == 0)
                {
                    DayOpen = _price;
                }
            }
        }

        public decimal DayOpen { get; set; }
        public decimal Change => Price - DayOpen;
        public double PercentChange => (double) Math.Round(Change/Price, 4);

    }
}