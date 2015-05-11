using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Text;

namespace SeleniumTests
{
    [TestFixture]
    public class TestCaseOne
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "https://prodaja.hac.hr/";
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheCaseOneTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/");
            driver.FindElement(By.Id("formPost_username")).Clear();
            driver.FindElement(By.Id("formPost_username")).SendKeys("juraj.klaric");
            driver.FindElement(By.Id("formPost_password")).Clear();
            driver.FindElement(By.Id("formPost_password")).SendKeys("kingict1");
            driver.FindElement(By.Id("formPost_login")).Click();
            driver.FindElement(By.Id("ajax_racuni_nadoplata_intro")).Click();
            driver.FindElement(By.LinkText("Nadoplata karticom")).Click();
            driver.FindElement(By.Id("iznosBrutto")).Clear();
            driver.FindElement(By.Id("iznosBrutto")).SendKeys("100");
            driver.FindElement(By.Id("ajax_form_button_cont")).Click();
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}