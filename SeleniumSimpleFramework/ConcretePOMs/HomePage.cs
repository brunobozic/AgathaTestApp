using OpenQA.Selenium;
using SeleniumSimpleFramework.PageObjectModel;
using System;

namespace SeleniumSimpleFramework.ConcretePOMs
{
    public class HomePage : BasePageObjectModel
    {
        #region Private properties

        private string _idOfUsernameTextInput = "formPost_username";
        private string _idOfPasswordTextInput = "formPost_password";
        private string _titleTextInitialPage = "HAC Portal";
        private string _idOfLogOffButton = "logOff_logoff";
        private string _headerTextNameOfLoggedInUser = "headerLineText";
        private string _url;

        # endregion

        #region Public properties

        public string TitleTextInitialPage
        {
            get { return _titleTextInitialPage; }
            set { _titleTextInitialPage = value; }
        }

        public string IdOfLogOffButton
        {
            get { return _idOfLogOffButton; }
            set { _idOfLogOffButton = value; }
        }

        public string HeaderTextNameOfLoggedInUser
        {
            get { return _headerTextNameOfLoggedInUser; }
            set { _headerTextNameOfLoggedInUser = value; }
        }

        public string IdOfUsernameTextInput
        {
            get { return _idOfUsernameTextInput; }
            set { _idOfUsernameTextInput = value; }
        }

        public string IdOfPasswordTextInput
        {
            get { return _idOfPasswordTextInput; }
            set { _idOfPasswordTextInput = value; }
        }

        public string ExpectedTitleText { get; set; }

        public new string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        #endregion Public properties

        #region CTOR

        /// <summary>
        /// This constructor is to be used when we are already logged in, and wish to reuse the logged in session
        /// that is contained within an existing selenium driver instance.
        /// </summary>
        /// <param name="seleniumDriver"></param>
        public HomePage(IWebDriver browser, string url)
            : base(browser, url)
        {
        }

        #endregion CTOR

        public bool NavigateToNadoplataKarticom()
        {
            if (base.IsElementPresentById("ajax_racuni_nadoplata_intro"))
            {
                base.GetElementById("ajax_racuni_nadoplata_intro").Click();

                if (base.IsElementPresentByLink("Nadoplata karticom"))
                {
                    base.GetElementByLink("Nadoplata karticom").Click();

                    if (base.IsElementPresentById("iznosBrutto"))
                    {
                        return true;
                    }
                    AddVerificationProblemAndTakeScreenshotById(String.Format("Missing Element by Link Text: [ {0} ]", "iznosBrutto"));
                }
                else
                {
                    AddVerificationProblemAndTakeScreenshotById(String.Format("Missing Element by Id: [ {0} ]", "Nadoplata karticom"));
                }
            }
            else
            {
                AddVerificationProblemAndTakeScreenshotById(String.Format("Missing Element by Id: [ {0} ]", "ajax_racuni_nadoplata_intro"));
            }

            return false;
        }

        public bool EnterThePaymentAmount(string iznos = "100")
        {
            base.GetElementById("iznosBrutto").Clear(); //  driver.FindElement(By.Id("iznosBrutto")).Clear();
            base.GetElementById("iznosBrutto").TypeText("100");  // driver.FindElement(By.Id("iznosBrutto")).SendKeys("100");
            base.GetElementById("ajax_form_button_cont").Click(); // driver.FindElement(By.Id("ajax_form_button_cont")).Click();

            return true;
        }

        protected override void ConstructUrl()
        {
            Url = @"https:\\prodaja.hac.hr";
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public new string GetPageTitle()
        {
            return GetTitle();
        }

        public override bool IsValidPageDisplayed()
        {
            if (GetTitle() == ExpectedTitleText) { return true; }
            AddVerificationProblemAndTakeScreenshot(string.Format("Expected title: [{0}], found title: [{1}]", ExpectedTitleText, GetPageTitle()), "ExpectedTitleNotFound");
            return false;
        }

        public new string GetTextByClassName(string p)
        {
            return base.GetTextByClassName(p);
        }

        public SignInPage LogOut()
        {
            SafeClickAndWaitForIdById(IdOfLogOffButton, IdOfPasswordTextInput);
            SignInPage sip = new SignInPage(this.Driver, this.Url);

            return sip;
        }
    }
}