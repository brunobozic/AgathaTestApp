using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumSimpleFramework.Interfaces;
using System;
using System.Text;

namespace SeleniumSimpleFramework.PageObjectModel
{
    public abstract class BasePageObjectModel : SeleniumSpecific
    {
        #region Private Properties

        private string _url = string.Empty;

   
        #endregion Private Properties

        #region Public Properties

    

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        #endregion Public Properties

        #region CTOR

        //protected BasePageObjectModel(Browser browser, double driverTimeOut)
        //{
        //    Driver = GetWebDriverForBrowser(browser, driverTimeOut);
        //    VerificationErrors = new StringBuilder();
        //}

        ///  <summary>
        ///
        ///  </summary>
        /// <param name="seleniumDriver"></param>
        /// <param name="url"></param>
        ///  <param name="browser"></param>
        ///  <param name="driverTimeOut"></param>
        //protected BasePageObjectModel(string url, Browser browser, double driverTimeOut)
        //{
        //    _url = url;
        //    Driver = GetWebDriverForBrowser(browser, driverTimeOut);
        //    VerificationErrors = new StringBuilder();
        //}

        protected BasePageObjectModel(IWebDriver seleniumDriver, string url)
        {
            if (seleniumDriver == null) throw new ArgumentNullException("seleniumDriver");
            if (url == null) throw new ArgumentNullException("url");
            _url = url;
            Driver = seleniumDriver;

            VerificationErrors = new StringBuilder();
        }

        #endregion CTOR

        /// <summary>
        /// Builds derived page URL based on the BaseURL and specyfic page URL.
        /// </summary>
        /// <returns></returns>
        protected abstract void ConstructUrl();

        /// <summary>
        /// Veryfies derived page is displayed correctly.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValidPageDisplayed()
        {
            if (!Driver.PageSource.Contains("Server Error in ")) return true;
            Assert.Fail(String.Format("Server error while navigating\r\n\r\n {0}.", Driver.PageSource));
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        public void NavigateTostartPage()
        {
            Driver.Navigate().GoToUrl(_url);
        }

        /// <summary>
        /// Will dispose of active IWebDriver driver instance...
        /// </summary>
        public override void Quit()
        {
            if (Driver == null) return;

            try
            {
                Driver.Quit();
                Driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }

            if (VerificationErrors.Length > 0)
            {
                // The test failed....
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetPageTitle()
        {
            return GetTitle();
        }
    }
}