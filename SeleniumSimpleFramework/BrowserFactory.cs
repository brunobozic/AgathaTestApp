using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;

namespace SeleniumSimpleFramework
{
    public class BrowserFactory
    {
        private static IWebDriver _browser;
        private static int _timeout;
        private static bool _implicitWait = true;
        private static string _mainWindowHandler;
        public static string MainWindowHandler
        {
            get { return _mainWindowHandler; }
            set { _mainWindowHandler = value; }
        }
        public static bool ImplicitWait
        {
            get { return _implicitWait; }
            set { _implicitWait = value; }
        }

        public static int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public static void FooMethod()
        {
            var firingDriver = new EventFiringWebDriver(new FirefoxDriver());
            firingDriver.ExceptionThrown += firingDriver_TakeScreenshotOnException;

            _browser = firingDriver;
            _browser.Navigate().GoToUrl("http://yizeng.me");

            // try find a non-existent element where NoSuchElementException should be thrown
            _browser.FindElement(By.CssSelector("#some_id .foo")); // a screenshot should be taken automatically
        }

        private static void firingDriver_TakeScreenshotOnException(object sender, WebDriverExceptionEventArgs e)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-hhmm-ss");
            _browser.TakeScreenshot().SaveAsFile("Exception-" + timestamp + ".png", ImageFormat.Png);
        }

        public static IWebDriver GetBrowser(string browser, int driverTimeOut = 10)
        {
            _timeout = driverTimeOut;

            switch (browser)
            {
                case "ChromeDriver":

                    _browser = new ChromeDriver();
                    break;

                case "InternetExplorerDriver":

                    _browser = new InternetExplorerDriver();

                    break;

                case "FirefoxDriver":
                    _browser = new FirefoxDriver();

                    break;
            }

            if (_browser != null)
            {
                _browser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(_timeout));
            }
            else
            {
                // AddError(string.Format("Missing [Driver] instance {0} ", "Selenium"));
            }

            Debug.Assert(_browser != null, "_browser != null");
            _browser.Manage().Window.Maximize();
            _mainWindowHandler = _browser.CurrentWindowHandle;

            return _browser;
        }

        private static InternetExplorerDriver StartInternetExplorer()
        {
            var internetExplorerOptions = new InternetExplorerOptions
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                InitialBrowserUrl = "about:blank",
                EnableNativeEvents = true
            };

            return new InternetExplorerDriver(Directory.GetCurrentDirectory(), internetExplorerOptions);
        }

        private static FirefoxDriver StartFirefox()
        {
            var firefoxProfile = new FirefoxProfile
            {
                AcceptUntrustedCertificates = true,
                EnableNativeEvents = true
            };

            return new FirefoxDriver(firefoxProfile);
        }

        private static ChromeDriver StartChrome()
        {
            var chromeOptions = new ChromeOptions();
            var defaultDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\..\Local\Google\Chrome\User Data\Default";

            if (Directory.Exists(defaultDataFolder))
            {
                ForceDelete(defaultDataFolder);
            }

            // DesiredCapabilities capability = DesiredCapabilities.Chrome();
            // capability.SetCapability(CapabilityType.AcceptSslCertificates, true);

            return new ChromeDriver(Directory.GetCurrentDirectory(), chromeOptions);
        }

        protected static void ResetAttributes(FileSystemInfo fileInfo)
        {
            try
            {
                fileInfo.Attributes = fileInfo.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
            }
            catch (Exception exception)
            {
                // Logger.LogInfrastructureError(string.Format(Messages.ErrorAttributesReset, fileInfo.FullName), exception);
            }
        }

        public static void ForceDelete(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            var baseFolder = new DirectoryInfo(path);

            foreach (var item in baseFolder.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                ResetAttributes(item);
            }

            foreach (var item in baseFolder.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                ResetAttributes(item);
            }

            try
            {
                baseFolder.Delete(true);
            }
            catch (Exception exception)
            {
                // Logger.LogInfrastructureError(string.Format(Messages.ErrorDirectoryDeletion, baseFolder.FullName), exception);
            }
        }

      
    }
}