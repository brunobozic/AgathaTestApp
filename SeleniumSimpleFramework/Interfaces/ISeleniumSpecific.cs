using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium.PhantomJS;
using Selenium;


namespace SeleniumSimpleFramework.Interfaces
{
    public abstract class SeleniumSpecific
    {
        /// <summary>
        ///
        /// </summary>
        protected const int TimeOut = 10;
  
        /// <summary>
        ///
        /// </summary>
        private const string LocationToStoreScreenshots = @"C://Screenshots//";

        /// <summary>
        /// The driver.
        /// </summary>
        protected IWebDriver Driver;

        private StringBuilder _verificationErrors;

        public StringBuilder VerificationErrors
        {
            get { return _verificationErrors; }
            set { _verificationErrors = value; }
        }

        public string NameOfTest { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="driverTimeOut"></param>
        /// <returns></returns>
        //protected abstract IWebDriver GetWebDriverForBrowser(Browser browser, double driverTimeOut);

        /// <summary>
        /// Will dispose of active IWebDriver driver instance...
        /// </summary>
        public abstract void Quit();

        internal virtual string GetTextByClassName(string p)
        {
            throw new NotImplementedException();
        }

        public virtual string GetTextById(string p)
        {
            return GetText(By.Id(p));
        }

        public virtual bool IsElementPresentById(string p)
        {
            return IsElementPresent(By.Id(p));
        }

        public virtual bool IsElementPresentByLink(string p)
        {
            return IsElementPresent(By.LinkText(p));
        }

        #region Screenshots and exception handling

        private void AddVerificationProblemAndTakeScreenshot(By elementLocator)
        {
            AddError(string.Format("Missing DOM element: [ {0} ], URL: [ {1} ]",
                elementLocator, Driver.Url));
            TakeScreenshot(string.Format("MissingDOMelement-{0}", elementLocator));
        }

        public void AddVerificationProblemAndTakeScreenshotById(string elementLocator)
        {
            AddError(string.Format("Missing DOM element: [ {0} ], URL: [ {1} ]",
                elementLocator, Driver.Url));
            TakeScreenshot(string.Format("MissingDOMelement-{0} ", elementLocator));
        }

        public void AddVerificationProblemAndTakeScreenshot(string error, string screenshotName)
        {
            AddError(string.Format(error));
            TakeScreenshot(screenshotName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nameOfScreenie"></param>
        public virtual void TakeScreenshot(string nameOfScreenie)
        {
            try
            {
                string nameOfScreenieSpliced = string.Empty;
                var screenshotDriver = Driver as ITakesScreenshot;
                if (screenshotDriver == null) return;
                var screen = screenshotDriver.GetScreenshot();

                string directoryNameTimeStampPart = string.Format("{0}{1}{2}", DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.Now.Day);

                if (!string.IsNullOrEmpty(NameOfTest))
                {
                    if (NameOfTest.Contains(Convert.ToChar("^")))
                    {
                        NameOfTest = NameOfTest.Split(Convert.ToChar("^"))[0];
                    }

                    nameOfScreenieSpliced = Regex.Replace(NameOfTest, "[^a-z0-9\\-_]+", "_", RegexOptions.IgnoreCase);
                }
                else
                {
                    NameOfTest = "Test";
                }

                string locationToStoreScreenshotsSpliced = string.Format("{0}{1}-{2}", LocationToStoreScreenshots, NameOfTest,
                       directoryNameTimeStampPart);

                if (!Directory.Exists(locationToStoreScreenshotsSpliced))
                {
                    try
                    {
                        Directory.CreateDirectory(locationToStoreScreenshotsSpliced);
                    }
                    catch (Exception ex)
                    {
                        AddError("Create Directory Exception", ex);
                    }
                }

                nameOfScreenie =
                    nameOfScreenie.Replace(".", string.Empty)
                        .Replace(":", string.Empty)
                        .Replace("_", string.Empty);

                nameOfScreenieSpliced = string.Format("{0}-{1}", nameOfScreenieSpliced, nameOfScreenie);

                screen.SaveAsFile(string.Format("{0}{1}{2}", locationToStoreScreenshotsSpliced, nameOfScreenieSpliced, ".png"), ImageFormat.Png);
            }
            catch (Exception ex)
            {
                AddError(ex.ToString());
            }
        }

        private void AddError(string p, Exception ex)
        {
            if (p == null) throw new ArgumentNullException("p");
        }

        protected void AddError(string error)
        {
            VerificationErrors.Append(string.Format("{0}: {1}", DateTime.Now, error));
        }

        #endregion Screenshots and exception handling

        #region Click

        /// <summary>
        /// Use when you are navigating via a hyper-link and need for the page to fully load before
        /// moving further.
        /// </summary>
        private void ClickAndWait(By locator, string newUrl)
        {
            Driver.FindElement(locator).Click();
            var wait = new WebDriverWait(Driver,
                TimeSpan.FromSeconds(TimeOut));
            wait.Until(d => d.Url.Contains(newUrl.Trim('~')));
        }

        private void SafeClickAndWait(By locator, string newUrl)
        {
            if (IsElementPresent(locator))
            {
                Driver.FindElement(locator).Click();
                var wait = new WebDriverWait(Driver,
                    TimeSpan.FromSeconds(TimeOut));
                wait.Until(d => d.Url.Contains(newUrl.Trim('~')));
            }
            else
            {
                AddVerificationProblemAndTakeScreenshot(locator);
            }
        }

        private void SafeClickAndWait(By locator, By locatorNew)
        {
            if (IsElementPresent(locator))
            {
                Driver.FindElement(locator).Click();
                var wait = new WebDriverWait(Driver,
                    TimeSpan.FromSeconds(TimeOut));
                wait.Until(d => d.IsElementPresent(locatorNew));
            }
            else
            {
                AddVerificationProblemAndTakeScreenshot(locator);
            }
        }

        private void SafeClick(By elementLocator)
        {
            if (IsElementPresent(elementLocator))
            {
                Click(elementLocator);
            }
            else
            {
                AddVerificationProblemAndTakeScreenshot(elementLocator);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        private void Click(By locator)
        {
            Driver.FindElement(locator).Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newUrl"></param>
        public virtual void ClickAndWaitById(string id, string newUrl)
        {
            ClickAndWait(By.Id(id), newUrl);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        public virtual void ClickById(string id)
        {
            Click(By.Id(id));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newUrl"></param>
        public virtual void SafeClickAndWaitById(string id, string newUrl)
        {
            SafeClickAndWait(By.Id(id), newUrl);
        }

        public virtual void SafeClickAndWaitForIdById(string id, string newId)
        {
            SafeClickAndWait(By.Id(id), By.Id(newId));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        public virtual void SafeClickById(string id)
        {
            SafeClick(By.Id(id));
        }

        #endregion Click

        #region Find elements / selectors

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IWebElement GetElementById(string id)
        {
            return Driver.FindElement(By.Id(id));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public virtual bool IsElementPresent(By by)
        {
            try
            {
                Driver.FindElement(@by);
                return true;
            }
            catch (NoSuchElementException)
            {
                // nsee.Message
                AddVerificationProblemAndTakeScreenshot(by);
                return false;
            }
        }

        public virtual IWebElement GetElementByLink(string p)
        {
            return Driver.FindElement(By.LinkText(p));
        }

        internal List<IWebElement> GetElementsByClass(string x)
        {
            return Driver.FindElements(By.ClassName(x)).ToList();
        }

        internal List<IWebElement> GetElementsByLink(string p)
        {
            return Driver.FindElements(By.LinkText(p)).ToList();
        }

        #endregion Find elements / selectors

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public virtual string GetValueOfElement(By locator)
        {
            return Driver.FindElement(locator).GetAttribute("value");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        public virtual void Uncheck(By locator)
        {
            var element = Driver.FindElement(locator);
            if (element.Selected)
                element.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        internal string GetPageSource()
        {
            return Driver.PageSource;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        internal string GetTitle()
        {
            return Driver.Title;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        public virtual void UncheckById(string id)
        {
            Uncheck(By.Id(id));
        }

        public virtual void SelectDropDownElement(string elementId, string valueToBeSelected)
        {
            var options = GetElementById(elementId).FindElements(By.TagName("option"));
            foreach (var option in options.Where(option => valueToBeSelected == option.Text))
            {
                option.Click();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        public virtual void TypeIntoElementId(string id, string text)
        {
            if (IsElementPresent(By.Id(id)))
            {
                var element = GetElementById(id);
                element.Clear();
                element.SendKeys(text);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public virtual string GetText(By locator)
        {
            return IsElementPresent(locator) ? Driver.FindElement(locator).Text : string.Format("No element found {0}", locator);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValueShouldBe"></param>
        /// <returns></returns>
        public virtual bool IsElementValueEqualTo(string fieldName, string fieldValueShouldBe)
        {
            var elementValue = Driver.FindElement(By.Name(fieldName)).GetAttribute("value");
            return elementValue.Equals(fieldValueShouldBe);
        }

        public virtual object ExecuteJavaScript(string javaScript, params object[] args)
        {
            var javaScriptExecutor = (IJavaScriptExecutor)Driver;

            return javaScriptExecutor.ExecuteScript(javaScript, args);
        }

        public virtual void NavigateBack()
        {
            Driver.Navigate().Back();
        }

        public virtual void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        public virtual void KeyDown(string key)
        {
            new Actions(Driver).KeyDown(key);
        }

        public virtual void KeyUp(string key)
        {
            new Actions(Driver).KeyUp(key);
        }

        public virtual void DragAndDrop(IWebElement source, IWebElement destination)
        {
            (new Actions(Driver)).DragAndDrop(source, destination).Build().Perform();
        }

        public virtual void ResizeWindow(int width, int height)
        {
            ExecuteJavaScript(string.Format("window.resizeTo({0}, {1});", width, height));
        }

        public virtual void SwitchToFrame(IWebElement inlineFrame)
        {
            Driver.SwitchTo().Frame(inlineFrame);
        }

        public virtual void SwitchToPopupWindow()
        {
            foreach (var handle in Driver.WindowHandles.Where(handle => handle != _mainWindowHandler)) // TODO:
            {
                Driver.SwitchTo().Window(handle);
            }
        }

        public virtual void SwitchToMainWindow()
        {
            Driver.SwitchTo().Window(_mainWindowHandler);
        }

        public virtual void SwitchToDefaultContent()
        {
            Driver.SwitchTo().DefaultContent();
        }

        public virtual void WaitAjax()
        {
            Contract.Assume(Driver != null);

            var ready = new Func<bool>(() => (bool)ExecuteJavaScript("return (typeof($) === 'undefined') ? true : !$.active;"));

            Contract.Assert(WaitHelper.SpinWait(ready, TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(100)));
        }

        public virtual void WaitReadyState()
        {
            Contract.Assume(Driver != null);

            var ready = new Func<bool>(() => (bool)ExecuteJavaScript("return document.readyState == 'complete'"));

            Contract.Assert(WaitHelper.SpinWait(ready, TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(100)));
        }

        public string _mainWindowHandler { get; set; }
    }

    public class WaitHelper
    {
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _checkInterval;
        private readonly Stopwatch _stopwatch;
        private bool _isSatisfied = true;

        private WaitHelper(TimeSpan timeout)
            : this(timeout, TimeSpan.FromSeconds(1))
        {
        }

        private WaitHelper(TimeSpan timeout, TimeSpan checkInterval)
        {
            Contract.Requires(timeout >= TimeSpan.Zero);
            Contract.Requires(checkInterval >= TimeSpan.Zero);

            _timeout = timeout;
            _checkInterval = checkInterval;
            _stopwatch = Stopwatch.StartNew();
        }

        public static WaitHelper WithTimeout(TimeSpan timeout, TimeSpan pollingInterval)
        {
            return new WaitHelper(timeout, pollingInterval);
        }

        public static WaitHelper WithTimeout(TimeSpan timeout)
        {
            return new WaitHelper(timeout);
        }

        public WaitHelper WaitFor(Func<bool> condition)
        {
            Contract.Requires(condition != null);

            if (!_isSatisfied)
            {
                return this;
            }

            while (!condition())
            {
                var sleepAmount = Min(_timeout - _stopwatch.Elapsed, _checkInterval);

                if (sleepAmount < TimeSpan.Zero)
                {
                    _isSatisfied = false;
                    break;
                }

                Thread.Sleep(sleepAmount);
            }

            return this;
        }

        public bool IsSatisfied
        {
            get { return _isSatisfied; }
        }

        public void EnsureSatisfied()
        {
            if (!_isSatisfied)
            {
                throw new TimeoutException();
            }
        }

        public void EnsureSatisfied(string message)
        {
            Contract.Requires(message != null);

            if (!_isSatisfied)
            {
                throw new TimeoutException(message);
            }
        }

        public static bool SpinWait(Func<bool> condition, TimeSpan timeout)
        {
            return SpinWait(condition, timeout, TimeSpan.FromSeconds(1));
        }

        public static bool SpinWait(Func<bool> condition, TimeSpan timeout, TimeSpan pollingInterval)
        {
            return WithTimeout(timeout, pollingInterval).WaitFor(condition).IsSatisfied;
        }

        public static bool Try(Action action)
        {
            Exception exception;

            return Try(action, out exception);
        }

        public static bool Try(Action action, out Exception exception)
        {
            Contract.Requires(action != null);

            try
            {
                action();
                exception = null;

                return true;
            }
            catch (Exception e)
            {
                exception = e;

                return false;
            }
        }

        public static Func<bool> MakeTry(Action action)
        {
            return () => Try(action);
        }

        private static T Min<T>(T left, T right) where T : IComparable<T>
        {
            return left.CompareTo(right) < 0 ? left : right;
        }
    }

  
}