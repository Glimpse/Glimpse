using System;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Glimpse.Test.MVCLoadTests
{
    public class MVCLoadTest:SeleniumTest
    {
        public MVCLoadTest() : base("source\\Glimpse.Mvc4.MusicStore.Sample") { }

        
        [Fact]
        public void LoadTest()
        {
            //Arrange
            const int totalCount = 20;
            const int intervalCount = 100;
            var timer = DateTime.Now;

            // Act

            Parallel.For(0, totalCount, i =>
            {
                var driver = new ChromeDriver("C:\\");
                driver.Url = GetAbsoluteUrl("/");

            for (int j = 0; j < intervalCount; j++)
                {
                    driver.Url = GetAbsoluteUrl("/Store/Browse?Genre=Jazz");
                    driver.Url = GetAbsoluteUrl("/Store/Browse?Genre=Electronic");
                    driver.Url = GetAbsoluteUrl("/Store/Browse?Genre=Alternative");
                    driver.Url = GetAbsoluteUrl("/Store/Browse?Genre=Classical");
                    driver.Url = GetAbsoluteUrl("/Store/Browse?Genre=Rap");
                    driver.Url = GetAbsoluteUrl("/Store/Browse?Genre=Latin");
                    driver.Url = GetAbsoluteUrl("/Store/Browse?Genre=Blues");
                    driver.Url = GetAbsoluteUrl("/Store/Browse?Genre=Metal");
                    driver.Url = GetAbsoluteUrl("/Store/Browse?Genre=Rock");
                    driver.Url = GetAbsoluteUrl("/Store");
                    driver.Url = GetAbsoluteUrl("/Account/Login?ReturnUrl=%2fStoreManager");
                }


                driver.Quit();
            });

            var timerFinish = DateTime.Now;
            var delta = timerFinish - timer;
            
            Console.WriteLine("Test took {0} hours, {1} minutes to run", delta.Hours, delta.Minutes);

            //Assert
            // If this test reaches this we have successfully run.
            Assert.True(true);
        }
    }
}
