using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeleniumSimpleFramework
{
    public static class DriverExtensions
    {
        public const int DefaultTimeout = 10;

        public static void Wait(this IWebDriver driver, int seconds = DefaultTimeout)
        {
            if (seconds <= 60)
                seconds *= 1000;

            System.Threading.Thread.Sleep(seconds);
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, Func<IWebElement, bool> predicate)
        {
            return driver.FindElements(by, predicate).First();
        }

        public static IEnumerable<IWebElement> FindElements(this IWebDriver driver, By by, Func<IWebElement, bool> predicate)
        {
            return driver.FindElements(by).Where(predicate);
        }

        public static IWebElement WaitForElement(this IWebDriver driver, By by, Func<IWebElement, bool> predicate = null, int seconds = DefaultTimeout)
        {
            return driver.WaitForElements(by, predicate, seconds).First();
        }

        public static IEnumerable<IWebElement> WaitForElements(this IWebDriver driver, By by, Func<IWebElement, bool> predicate = null, int seconds = DefaultTimeout)
        {
            IEnumerable<IWebElement> els;
            var retry = 0;

            do
            {
                retry++;
                driver.Wait(1);

                els = driver.FindElements(by);
                if (predicate != null)
                    els = els.Where(predicate);
            } while (els != null && (!els.Any() && retry < seconds));

            return els;
        }

        public static IJavaScriptExecutor GetJavaScriptExecutor(this IWebDriver driver)
        {
            return driver as IJavaScriptExecutor;
        }
    }

    public static class OtherExtensions
    {
        /// <summary>
        /// Set focus to the HTML input control and type all characters from passed string one by one.
        /// </summary>
        /// <param name="inputControl">Input to type text on.</param>
        /// <param name="text">Text to be typed to the control.</param>
        public static void TypeText(this IWebElement inputControl, string text)
        {
            while (true)
            {
                if (inputControl == null)
                {
                    throw new ArgumentNullException("inputControl");
                }

                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new ArgumentNullException("text");
                }

                inputControl.Clear();

                Assert.IsTrue(!inputControl.Enabled, "Control is disabled or read-only.");
            }
        }

        // FindAll
        // FindById

        ///  <summary>
        ///
        ///  </summary>
        /// <param name="driver"></param>
        /// <param name="id"></param>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public static IWebElement GetElementById(this IWebElement webElement, IWebDriver driver, string id)
        {
            return driver.FindElement(By.Id(id));
        }

        // This method finds a select element and then selects
        // the option element using the visible text
        public static void FindSelectElement(this IWebDriver driver, By bylocatorForSelectElement, String text)
        {
            IWebElement selectElement = driver.FindElement(bylocatorForSelectElement);
            // selectElement.FindElement(By.XPath("//option[contains(text(), '" + text + "')]")).Selected;
        }     //// This is a basic wait for element not present a'la Selenium RC

        //// but sharing the same timeout value as the driver
        //public static void WaitForElementNotPresent(this IWebDriver driver, By bylocator)
        // {
        //     int timeoutinteger = SeleniumFWCommon.DriverTimeout.Seconds;

        //     for (int second = 0; ; second++)
        //     {
        //         Thread.Sleep(1000);

        //         if (second == timeoutinteger) Assert.Fail("Timeout: Element still visible at: " + bylocator);
        //         try
        //         {
        //             IWebElement element = driver.FindElement(bylocator);
        //         }
        //         catch (NoSuchElementException)
        //         {
        //             break;
        //         }
        //     }
        // }
        /// <summary>
        /// Determines whether the locator returns an element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="locator">The locator.</param>
        /// <returns>
        ///   <c>true</c> if the eleemnt is present; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsElementPresent(this IWebElement element, By locator)
        {
            try
            {
                element.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the locator returns an element.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <returns>
        ///   <c>true</c> if the eleemnt is present; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsElementPresent(this IWebDriver driver, By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }

    public static class WebElementExtensions
    {
        private static readonly StringComparer DefaultComparer =
                StringComparer.InvariantCultureIgnoreCase;

        private static readonly string[] Selected = new[] { "true", "selected" };

        public static bool IsSelected(this IWebElement element)
        {
            var attribute = element.GetAttribute("selected");
            return Selected.Contains(attribute, DefaultComparer);
        }

        private static readonly string[] Checked = new[] { "true", "checked" };

        public static bool IsChecked(this IWebElement element)
        {
            var attribute = element.GetAttribute("checked");
            return Checked.Contains(attribute, DefaultComparer);
        }

        public static void SetChecked(this IWebElement element)
        {
            if (!element.IsChecked())
                element.Click();
        }

        public static void SetUnchecked(this IWebElement element)
        {
            if (element.IsChecked())
                element.Click();
        }

        private const StringComparison DefaultComparison =
            StringComparison.InvariantCultureIgnoreCase;

        public static IWebElement GetOptionByValue(
            this IWebElement element,
            string value)
        {
            return element.GetOption(
                o => value.Equals(o.GetAttribute("value"), DefaultComparison));
        }

        public static IWebElement GetOptionByText(
            this IWebElement element,
            string text)
        {
            return element.GetOption(o => text.Equals(o.Text, DefaultComparison));
        }

        private static IWebElement GetOption(
            this IWebElement element,
            Func<IWebElement, bool> predicate)
        {
            return element
                .FindElements(By.CssSelector("option"))
                .FirstOrDefault(predicate);
        }
    }
}