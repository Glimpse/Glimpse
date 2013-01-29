using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Glimpse.Test.Integration
{
    public class BrowserFactory : IDisposable
    {
        public BrowserFactory()
        {
            Browser = new FirefoxDriver();
        }

        public IWebDriver Browser { get; set; }

        public void Dispose()
        {
            Browser.Dispose();
        }
    }
}