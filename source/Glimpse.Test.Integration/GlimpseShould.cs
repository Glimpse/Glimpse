using OpenQA.Selenium;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Integration
{
    public class GlimpseShould : IUseFixture<BrowserFactory>
    {
        private IWebDriver browser;

        [Theory, ExcelData(@"Tests.xls", "SELECT * FROM Urls")]
        public void Integration(string url, string expected, string tabSelector, string elementSelector, bool skip = false)
        {
            if (skip)
            {
                return;
            }

            browser.Navigate().GoToUrl(url);

            OpenGlimpse();

            ClickTab(tabSelector);

            var actual = GetElementText(elementSelector);

            if (!actual.Equals(expected))
            {
                browser.SaveScreenshot(url);
            }

            Assert.Equal(expected, actual);
        }

        public void SetFixture(BrowserFactory factory)
        {
            browser = factory.Browser;
        }

        private void OpenGlimpse()
        {
            try
            {
                IWebElement glimpse = browser.FindElement(By.CssSelector(".glimpse-open"), 10);
                HighlightElement(browser, glimpse);
                glimpse.Click();
            }
            catch (ElementNotVisibleException)
            {
                // swallow, Glimpse is already open
            }
        }

        private void ClickTab(string tabSelector)
        {
            var tab = browser.FindElement(By.CssSelector(string.IsNullOrEmpty(tabSelector) ? ".glimpse-tabitem-test-tab" : tabSelector), 5);
            HighlightElement(browser, tab);
            tab.Click();
        }

        private string GetElementText(string elementSelector)
        {
            IWebElement element = browser.FindElement(By.CssSelector(string.IsNullOrEmpty(elementSelector) ? "#__intTestId" : elementSelector), 5);
            HighlightElement(browser, element);
            var text = element.Text;
            return text;
        }

        private void HighlightElement(IWebDriver driver, IWebElement element)
        {
            var js = driver as IJavaScriptExecutor;

            if (js != null)
            {
                js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, "color: yellow; border: 2px solid red;");
                js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, string.Empty);
            }
        }
    }
}