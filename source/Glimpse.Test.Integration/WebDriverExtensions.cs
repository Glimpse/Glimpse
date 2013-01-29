using System;
using System.Drawing.Imaging;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Glimpse.Test.Integration
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }

            return driver.FindElement(by);
        }

        public static void SaveScreenshot(this IWebDriver browser, string url)
        {
            var camera = browser as ITakesScreenshot;

            if (camera != null)
            {
                var screenshot = camera.GetScreenshot();

                Array.ForEach(Path.GetInvalidFileNameChars(), c => url = url.Replace(c.ToString(), string.Empty));
                url = url.Replace("http", string.Empty);

                screenshot.SaveAsFile(url + ".png", ImageFormat.Png);
            }
        }
    }
}