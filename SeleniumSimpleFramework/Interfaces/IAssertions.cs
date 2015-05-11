using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSimpleFramework.Interfaces
{
    public interface IAssertions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        void AssertTextContains(string id, string text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="text"></param>
        void AssertTextContains(By locator, string text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        void AssertTextEquals(string id, string text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="text"></param>
        void AssertTextEquals(By locator, string text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        void AssertValueContains(string id, string text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="text"></param>
        void AssertValueContains(By locator, string text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        void AssertValueEquals(string id, string text);

      
    }
}
