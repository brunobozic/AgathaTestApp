using System;
using Selenium;

namespace SeleniumSimpleFramework.PageObjectModel
{
    public class Label : ControlBase
    {
        private readonly string _selector;

        public Label(ISelenium selenium, string selector)
            : base(selenium)
        {
            _selector = selector;
        }

        public string GetText()
        {
            return Selenium.GetText(_selector);
        }

        public string GetSelector()
        {
             return _selector; 
        }

        public override String ToString()
        {
            return "Label with text '" + GetText() + "'";
        }
    }
}