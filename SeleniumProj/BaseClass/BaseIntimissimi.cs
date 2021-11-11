using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumProj.BaseClass
{
    public class BaseIntimissimi
    {
        public IWebDriver driver;

        [OneTimeSetUp()]
        public void Open()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
            driver = new ChromeDriver(chromeOptions)
            {
                Url = "https://test.intimissimi.com/us/login"
            };
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60);
        }

        [OneTimeTearDown()]
        public void Close()
        {
            Console.WriteLine("End Testing");
            
        }
    }
}
