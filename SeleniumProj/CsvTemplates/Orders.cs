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
        public string Brand { get; set; }
        public string Locale { get; set; }
        
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string LastName { get; set; }
        public string ShippingMethod { get; set; }

        public string PaymentMethod { get; set; }
        public string PaymentAmount { get; set; }
        public string SkuAndAttribute { get; set; }
        public bool IsRegistered { get; set; }
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

        public static List<Orders> AddOrder(string orderNumber, string lastName, string paymentMethod)
        {
            return new List<Orders>
            {
                new Orders
                {
                    //OrderId = id,
                    OrderNumber = orderNumber,
                    OrderDate = DateTime.Now,
                    LastName = lastName,
                    PaymentMethod = paymentMethod
                }
            };
        }

        public static List<Orders> AddOrder(string orderNumber, string lastName, string paymentMethod, string paymentAmount)
        {
            return new List<Orders>
            {
                new Orders
                {
                    //OrderId = id,
                    OrderNumber = orderNumber,
                    OrderDate = DateTime.Now,
                    LastName = lastName,
                    PaymentMethod = paymentMethod,
                    PaymentAmount = paymentAmount
                }
            };
        }

        public static List<Orders> AddOrder(string brand, string locale, string orderNumber, string lastName, string shippingMethod, string paymentMethod, string paymentAmount, string skuAndAttribute, bool isRegistered)
        {
            {
                return new List<Orders>
            {
                new Orders
                {
                    //OrderId = id,
                    Brand = brand,
                    Locale = locale,
                    OrderNumber = orderNumber,
                    OrderDate = DateTime.Now,
                    LastName = lastName,
                    ShippingMethod = shippingMethod,
                    PaymentMethod = paymentMethod,
                    PaymentAmount = paymentAmount,
                    SkuAndAttribute = skuAndAttribute,
                    IsRegistered = isRegistered
                }
            };
            }
        }
    }
}

