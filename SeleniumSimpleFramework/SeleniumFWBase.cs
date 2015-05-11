using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using SeleniumSimpleFramework.PageObjectModel;

namespace SeleniumSimpleFramework
{
  

    public class SitePageObject
    {
        /// <summary>
        /// The driver.
        /// </summary>
        private readonly IWebDriver _driver;

        /// <summary>
        /// Site URL
        /// </summary>
        private readonly string _url;

        public SitePageObject(IWebDriver driver)
        {
            this._driver = driver;
        }

        public SitePageObject(IWebDriver driver, string url)
        {
            this._driver = driver;
            this._url = url;
        }

        public void GoTo()
        {
            this._driver.Navigate().GoToUrl(_url);
        }

        /// <summary>
        /// Gets the login modal box form.
        /// </summary>
        /// <value>
        /// The login modal box form.
        /// </value>
        public LoginForm LoginForm
        {
            get
            {
                return new LoginForm(this._driver.FindElement(By.Id("modal_container")));
            }
        }

        public void TakesScreenshot(IWebDriver driver)
        {
            var screenshotDriver = driver as ITakesScreenshot;
            if (screenshotDriver == null) return;
            Screenshot screen = screenshotDriver.GetScreenshot();
            const string location = @"D://Screenshots/error1.png";
            screen.SaveAsFile(location, ImageFormat.Png);
        }

        public void VerifyAccess()
        {
            try
            {
                IWebElement element = this._driver.FindElement(By.TagName("tag"));
                Assert.AreEqual(element.Text, " Successfully Created the Account ! ");
            }
            catch (Exception e)
            {
                TakesScreenshot(this._driver);
                throw e;
            }
        }

        public bool IsTitleEqualTo(string Title)
        {
            Assert.That(this._driver.Title, Is.EqualTo(Title), string.Format("Failed due to title being [{0}]", this._driver.Title));

            return true;
        }

        public bool IsElementValueEqualTo(string fieldName, string fieldValueShouldBe)
        {
            var elementValue = this._driver.FindElement(By.Name(fieldName)).GetAttribute("value");
            Assert.That(elementValue.Equals(fieldValueShouldBe), Is.True, string.Format("Failed due to value being [{0}]", elementValue));

            return true;
        }

        public void Quit()
        {
            this._driver.Quit();
            this._driver.Dispose();
        }
    }

    public static class Helpers
    {
        //private static IWebDriver CreateDriverInstance(string baseUrl = BaseUrl)
        //{
        //    return new FirefoxDriver();
        //}

        //private static bool IsCurrentlyLoggedInAs(Login login)
        //{
        //    return _currentlyLoggedInAs != null &&
        //           _currentlyLoggedInAs.Equals(login);
        //}

        //private static void Logon(Login login)
        //{
        //    StaticDriver.Navigate().GoToUrl(BaseUrl + VirtualPath + "/Logon.aspx");

        //    StaticDriver.FindElement(By.Id("userId")).SendKeys(login.Username);
        //    StaticDriver.FindElement(By.Id("password")).SendKeys(login.Password);
        //    StaticDriver.FindElement(By.Id("btnLogin")).Click();

        //    _currentlyLoggedInAs = login;
        //}

        //private static void Logoff()
        //{
        //    // StaticDriver.Navigate().GoToUrl(
        //    //     VirtualPath + RedirectLinks.SignOff.Trim('~'));
        //    // _currentlyLoggedInAs = null;
        //}
    }

    public class LoginForm
    {
        /// <summary>
        /// The base element that holds the form on the page.
        /// </summary>
        private readonly IWebElement _baseElement;

        public LoginForm(IWebElement baseElement)
        {
            this._baseElement = baseElement;
        }

        /// <summary>
        /// Sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email
        {
            set
            {
                this._baseElement.FindElement(By.Id("Email")).SendKeys(value);
            }
        }

        /// <summary>
        /// Sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string UserName
        {
            set
            {
                this._baseElement.FindElement(By.Id("username")).SendKeys(value);
            }
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            set
            {
                this._baseElement.FindElement(By.Id("Password")).SendKeys(value);
            }
        }

        /// <summary>
        /// Clicks the on the submit button
        /// </summary>
        public void ClickOnSubmit(string CSSSelector)
        {
            this._baseElement.FindElement(By.CssSelector(CSSSelector)).Click();
        }
    }

    public class ModalBox<T>
    {
        /// <summary>
        /// The base element.
        /// </summary>
        private readonly IWebElement _baseElement;

        private readonly Func<IWebElement, T> _formConstructDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalBox{T}" /> class.
        /// </summary>
        /// <param name="baseElement">The base element.</param>
        /// <param name="formConstructDelegate">The form construction delegate that instantiates the form class passed in.</param>
        public ModalBox(IWebElement baseElement, Func<IWebElement, T> formConstructDelegate)
        {
            this._baseElement = baseElement;
            this._formConstructDelegate = formConstructDelegate;
            // Some code to wait for the modal box to appear here
        }

        public T Form
        {
            get { return this._formConstructDelegate.Invoke(this._baseElement); }
        }

        /// <summary>
        /// Gets the error messages inside the modal.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetErrors()
        {
            return this._baseElement.FindElements(By.CssSelector("span.field-validation-error")).Select(element => element.Text);
        }

        /// <summary>
        /// Clicks the on the submit button in the modal.
        /// </summary>
        /// <param name="expectModalClose">if set to <c>true</c> the modal box is expected to close.</param>
        public void ClickOnSubmit(bool expectModalClose)
        {
            this._baseElement.FindElement(By.CssSelector("input[type='submit']")).Click();
            if (expectModalClose)
            {
                // Some logic to wait for the modal box to close
            }
        }
    }
}