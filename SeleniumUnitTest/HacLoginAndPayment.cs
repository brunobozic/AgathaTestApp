using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using SeleniumSimpleFramework.ConcretePOMs;
using System.Text.RegularExpressions;

namespace SeleniumUnitTest
{
    /// <summary>
    /// Ovdje dođe novi text fixture
    /// </summary>
    /// <typeparam name="TWebDriver"></typeparam>
    [TestFixture]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(ChromeDriver))]
    public class Login<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        internal IWebDriver Browser;
        public SignInPage HACLoginPage;
        public HomePage HACHomePage;

        [TestFixtureSetUp]
        public void InstantiateSeleniumDriver()
        {
            Browser = BrowserFactory.GetBrowser(typeof(TWebDriver).Name, 10);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            if (Browser != null)
            {
                Browser.Quit();
            }
        }

        [SetUp]
        public void Setup()
        {
            HACLoginPage = new SignInPage(Browser, "https:\\prodaja.hac.hr");
        }

        [TearDown]
        public void Teardown()
        {
            if (HACLoginPage != null)
            {
                // Take screen on failure
                if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
                {
                    string fileName = Regex.Replace(TestContext.CurrentContext.Test.FullName, "[^a-z0-9\\-_]+", "_", RegexOptions.IgnoreCase);
                    HACLoginPage.TakeScreenshot(fileName);
                }

                HACLoginPage.Quit();
            }

            if (HACHomePage == null) return;

            // Take screen on failure
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
            {
                string fileName = Regex.Replace(TestContext.CurrentContext.Test.FullName, "[^a-z0-9\\-_]+", "_", RegexOptions.IgnoreCase);
                HACHomePage.TakeScreenshot(fileName);
            }

            HACHomePage.Quit();
        }

        [Test]
        [Description("Successfull Login to prodaja.hac.hr")]
        [Category("prodaja.hac.hr"), Category("Log in")]
        [TestCase("testuser", "testuser1")]
        [TestCase("kita", "odovce")]
        public void SuccesfullLogIn(string login, string password)
        {
            // For logging and screenshots...
            HACHomePage.NameOfTest = TestContext.CurrentContext.Test.FullName;

            // Go to start page...
            HACLoginPage.NavigateTostartPage();

            // Sanity check...
            Assert.AreEqual(HACLoginPage.IsValidPageDisplayed(), true, string.Format("Expected title: [{0}], found title: [{1}]", HACLoginPage.ExpectedTitleText, HACLoginPage.GetPageTitle()));
            Assert.AreEqual(HACLoginPage.IsElementPresentById(HACLoginPage.IdOfUsernameTextInput), true, string.Format("Expected element: [{0}] not found.", HACLoginPage.IdOfUsernameTextInput));
            Assert.AreEqual(HACLoginPage.IsElementPresentById(HACLoginPage.IdOfPasswordTextInput), true, string.Format("Expected element: [{0}] not found.", HACLoginPage.IdOfPasswordTextInput));
            Assert.AreEqual(HACLoginPage.IsElementPresentById(HACLoginPage.IdOfSubmitbutton), true, string.Format("Expected element: [{0}] not found.", HACLoginPage.IdOfSubmitbutton));

            // Enter UserName, followed by password, and finally click on the submit button....
            HACHomePage = HACLoginPage.LogInSpecificUser(login, password);

            // Sanity check...
            Assert.IsTrue(HACHomePage.IsElementPresentById(HACHomePage.IdOfLogOffButton), "Login was unsuccessful");
            Assert.AreEqual(HACHomePage.GetTextByClassName(HACHomePage.HeaderTextNameOfLoggedInUser), login);
            Assert.AreEqual(HACHomePage.IsElementPresentById(HACHomePage.IdOfLogOffButton), true);

            // TODO: logout
            HACLoginPage = HACHomePage.LogOut();

            // Check if we are indeed logged out
            Assert.IsFalse(HACLoginPage.IsElementPresentById(HACHomePage.IdOfLogOffButton), "Login was unsuccessful");
        }

        [Test]
        [Description("Unsuccessfull login to prodaja.hac.hr")]
        [Category("prodaja.hac.hr"), Category("Log in")]
        [TestCase("kita", "odovce")]
        [TestCase("testuser", "testuser1")]
        public void UnSuccesfullLogIn(string login, string password)
        {
            // For logging and screenshots...
            HACHomePage.NameOfTest = TestContext.CurrentContext.Test.FullName;

            // Go to start page...
            HACLoginPage.NavigateTostartPage();

            // Sanity check...
            Assert.AreEqual(HACLoginPage.IsValidPageDisplayed(), true, string.Format("Expected title: [{0}], found title: [{1}]", HACLoginPage.ExpectedTitleText, HACLoginPage.GetPageTitle()));
            Assert.AreEqual(HACLoginPage.IsElementPresentById(HACLoginPage.IdOfUsernameTextInput), true, string.Format("Expected element: [{0}] not found.", HACLoginPage.IdOfUsernameTextInput));
            Assert.AreEqual(HACLoginPage.IsElementPresentById(HACLoginPage.IdOfPasswordTextInput), true, string.Format("Expected element: [{0}] not found.", HACLoginPage.IdOfPasswordTextInput));
            Assert.AreEqual(HACLoginPage.IsElementPresentById(HACLoginPage.IdOfSubmitbutton), true, string.Format("Expected element: [{0}] not found.", HACLoginPage.IdOfSubmitbutton));

            // Enter UserName, followed by password, and finally click on the submit button....
            HACHomePage = HACLoginPage.LogInSpecificUser(login, password);

            // Sanity check...
            Assert.IsTrue(HACHomePage.IsElementPresentById(HACHomePage.IdOfLogOffButton), "Login was unsuccessful");
            Assert.AreEqual(HACHomePage.GetTextByClassName(HACHomePage.HeaderTextNameOfLoggedInUser), login);
            Assert.AreEqual(HACHomePage.IsElementPresentById(HACHomePage.IdOfLogOffButton), true);

            // TODO: logout
            HACLoginPage = HACHomePage.LogOut();

            // Check if we are indeed logged out
            Assert.IsFalse(HACLoginPage.IsElementPresentById(HACHomePage.IdOfLogOffButton), "Login was unsuccessful");
        }

        [TestFixture]
        [TestFixture(typeof(FirefoxDriver))]
        [TestFixture(typeof(InternetExplorerDriver))]
        [TestFixture(typeof(ChromeDriver))]
        public class LoginAndMakePayment<TWebDriver> where TWebDriver : IWebDriver, new()
        {
            internal IWebDriver Browser;
            public SignInPage HACLoginPage;
            public HomePage HACHomePage;

            [TestFixtureSetUp]
            public void InstantiateSeleniumDriver()
            {
                Browser = BrowserFactory.GetBrowser(typeof(TWebDriver).Name, 10);
            }

            [TestFixtureTearDown]
            public void FixtureTearDown()
            {
                if (Browser != null)
                {
                    Browser.Quit();
                }
            }

            [SetUp]
            public void Setup()
            {
                HACLoginPage = new SignInPage(Browser, "https:\\prodaja.hac.hr");
            }

            [TearDown]
            public void Teardown()
            {
                if (HACLoginPage != null)
                {
                    // Take screen on failure
                    if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
                    {
                        string fileName = Regex.Replace(TestContext.CurrentContext.Test.FullName, "[^a-z0-9\\-_]+", "_", RegexOptions.IgnoreCase);
                        HACLoginPage.TakeScreenshot(fileName);
                    }

                    HACLoginPage.Quit();
                }

                if (HACHomePage == null) return;
                // Take screen on failure
                if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
                {
                    string fileName = Regex.Replace(TestContext.CurrentContext.Test.FullName, "[^a-z0-9\\-_]+", "_", RegexOptions.IgnoreCase);
                    HACHomePage.TakeScreenshot(fileName);
                }

                HACHomePage.Quit();
            }

            [Test]
            public void The_Logout_Button_Should_Not_Be_Present()
            {
                Assert.IsFalse(HACHomePage.IsElementPresentById(HACHomePage.IdOfLogOffButton));
            }

            [Test]
            public void The_Logout_Button_Should_Be_Present()
            {
                Assert.IsTrue(HACHomePage.IsElementPresentById(HACHomePage.IdOfLogOffButton));
            }

            [Test]
            [Description("Test google search")]
            [Category("google"), Category("search")]
            [TestCase("10")]
            [TestCase("100")]
            public void LogInAndMakePayment(string amount)
            {
                // Go to start page...
                HACLoginPage.NavigateTostartPage();

                // Sanity check...
                Assert.AreEqual(HACLoginPage.IsValidPageDisplayed(), true, string.Format("Expected title: [{0}], found title: [{1}]", HACLoginPage.ExpectedTitleText, HACLoginPage.GetPageTitle()));
                Assert.AreEqual(HACLoginPage.IsElementPresentById(HACLoginPage.IdOfUsernameTextInput), true, string.Format("Expected element: [{0}] not found.", HACLoginPage.IdOfUsernameTextInput));
                Assert.AreEqual(HACLoginPage.IsElementPresentById(HACLoginPage.IdOfPasswordTextInput), true, string.Format("Expected element: [{0}] not found.", HACLoginPage.IdOfPasswordTextInput));
                Assert.AreEqual(HACLoginPage.IsElementPresentById(HACLoginPage.IdOfSubmitbutton), true, string.Format("Expected element: [{0}] not found.", HACLoginPage.IdOfSubmitbutton));

                // Enter UserName, followed by password, and finally click on the submit button....
                HACHomePage = HACLoginPage.LoginValidUser();

                // For logging and screenshots...
                HACHomePage.NameOfTest = TestContext.CurrentContext.Test.FullName;

                // Sanity check...
                Assert.IsTrue(HACHomePage.IsElementPresentById(HACHomePage.IdOfLogOffButton), "Login was unsuccessful");
                Assert.AreEqual(HACHomePage.GetTextByClassName(HACHomePage.HeaderTextNameOfLoggedInUser), "JURAJ.KLARIC");
                Assert.AreEqual(HACHomePage.IsElementPresentById(HACHomePage.IdOfLogOffButton), true);

                // Navigate to another view...
                HACHomePage.NavigateToNadoplataKarticom();

                // Sanity check...
                Assert.AreEqual(HACHomePage.IsElementPresentById(HACHomePage.IdOfLogOffButton), true);

                // je li dostupan input box ako nije screenshot i test fail
                // unesi iznos, ako se desi greska screenshot i test fail
                // je li dostupan submit, ako nije screenshot i test fail
                // klikni submit
                HACHomePage.EnterThePaymentAmount(amount);
            }
        }
    }
}