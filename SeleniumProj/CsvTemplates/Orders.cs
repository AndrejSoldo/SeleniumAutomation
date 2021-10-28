using System;
using System.Collections.Generic;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace SeleniumProj.CsvTemplates
{
    public class Orders
    {
        //public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public string LastName { get; set; }
        //public List<Orders> OrderList { get; set; }

        public Orders()
        {

        }

        public static List<Orders> GetOrders()
        {
            return new List<Orders> {
                new Orders
                {
                    //OrderId = $"0",
                    OrderNumber = $"10000000001",
                    OrderDate = DateTime.Now
                }
            };
        }

        public static List<Orders> AddOrder(string orderNumber)
        {
            return new List<Orders>
            {
                new Orders
                {
                    //OrderId = id,
                    OrderNumber = orderNumber,
                    OrderDate = DateTime.Now
                }
            };
        }

        public static List<Orders> AddOrder(string orderNumber, string lastName)
        {
            return new List<Orders>
            {
                new Orders
                {
                    //OrderId = id,
                    OrderNumber = orderNumber,
                    OrderDate = DateTime.Now,
                    LastName = lastName
                }
            };
        }
    }
}
