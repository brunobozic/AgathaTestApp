using OpenQA.Selenium;

namespace SeleniumSimpleFramework.Interfaces
{
    public interface ITestActions
    {
        /// <summary>
        ///
        /// </summary>
        void NavigateToPage();

        /// <summary>
        ///
        /// </summary>
        /// <param name="nameOfScreenie"></param>
        void TakeScreenshot(string nameOfScreenie);

        /// <summary>
        ///
        /// </summary>
        void VerifyAccess();

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        bool IsTitleEqualTo(string title);

        /// <summary>
        ///
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        bool IsElementPresent(By by);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValueShouldBe"></param>
        /// <returns></returns>
        bool IsElementValueEqualTo(string fieldName, string fieldValueShouldBe);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        void Click(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        void Click(By locator);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newUrl"></param>
        void ClickAndWait(string id, string newUrl);

        /// <summary>
        /// Use when you are navigating via a hyper-link and need for the page to fully load before
        /// moving further.
        /// </summary>
        void ClickAndWait(By locator, string newUrl);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IWebElement GetElementById(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        string GetValue(By locator);

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        string GetText(By locator);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        void TypeIntoElementId(string id, string text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        void UncheckById(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        void Uncheck(By locator);

        /// <summary>
        ///
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="valueToBeSelected"></param>
        void SelectDropDownElement(string elementId, string valueToBeSelected);
    }
}