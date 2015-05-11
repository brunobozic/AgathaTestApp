using OpenQA.Selenium;
using SeleniumSimpleFramework.PageObjectModel;

namespace SeleniumSimpleFramework.ConcretePOMs
{
    public class SignInPage : BasePageObjectModel
    {
        #region Private properties

        private string _idOfSubmitbutton = "formPost_login";
        private string _idOfUsernameTextInput = "formPost_username";
        private string _idOfPasswordTextInput = "formPost_password";
        private string _userName = "juraj.klaric";
        private string _password = "kingict1";
        private string _logoffButton = "logOff_logoff";
        private string _headerTextNameOfLoggedInUser = "headerLineText";
        private string _expectedTitleText = "HAC Prodaja";
        private string _url = "https:\\prodaja.hac.hr";

        #endregion Private properties

        #region Public properties

        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string IdOfSubmitbutton
        {
            get { return _idOfSubmitbutton; }
            set { _idOfSubmitbutton = value; }
        }

        public string LogoffButton
        {
            get { return _logoffButton; }
            set { _logoffButton = value; }
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

        public string ExpectedTitleText
        {
            get { return _expectedTitleText; }
            set { _expectedTitleText = value; }
        }

        #endregion Public properties

        #region CTOR

        public SignInPage(IWebDriver browser, string url)
            : base(browser, url)
        {
        }

        #endregion CTOR

        /// <summary>
        /// Sets the user name.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public void SetUserName()
        {
            base.TypeIntoElementId(IdOfUsernameTextInput, _userName);
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public void SetPassword()
        {
            base.TypeIntoElementId(IdOfPasswordTextInput, _password);
        }

        /// <summary>
        /// Clicks the on the submit button
        /// </summary>
        public void ClickOnSubmit()
        {
            base.ClickById(IdOfSubmitbutton);
        }

        protected override void ConstructUrl()
        {
            Url = @"https:\\prodaja.hac.hr";
        }

        public HomePage LoginValidUser()
        {
            SetUserName();
            SetPassword();
            ClickOnSubmit();

            return new HomePage(base.Driver, base.Url);
        }

        public override bool IsValidPageDisplayed()
        {
            if (base.IsElementPresentById(LogoffButton))
            {
                return true;
            }

            AddVerificationProblemAndTakeScreenshot(string.Format("Expected to find log out button: [{0}]", LogoffButton), "Sign Out Button Not Found");
            return false;
        }

        public void AddErrMsgAndTakeScreenshot(string error, string screenshotName)
        {
            AddVerificationProblemAndTakeScreenshot(error, screenshotName);
        }

        public HomePage LogInSpecificUser(string login, string password)
        {
            SetUserName(login);
            SetPassword(password);
            ClickOnSubmit();

            return new HomePage(base.Driver, base.Url);
        }

        private void SetPassword(string password)
        {
            base.TypeIntoElementId(IdOfPasswordTextInput, _password);
        }

        private void SetUserName(string login)
        {
            base.TypeIntoElementId(IdOfUsernameTextInput, _userName);
        }
    }
}