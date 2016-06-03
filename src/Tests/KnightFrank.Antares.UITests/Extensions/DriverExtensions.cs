namespace KnightFrank.Antares.UITests.Extensions
{
    using System;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public static class DriverExtensions
    {
        public static void WaitForAngularToFinish(this IWebDriver webDriver)
        {
            WaitForAngularToFinish(webDriver, BaseConfiguration.MediumTimeout);
        }

        public static void WaitForAngularToFinish(this IWebDriver webDriver, double timeout)
        {
            webDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(timeout));
            try
            {
                new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout)).Until(
                    driver =>
                    {
                        var javaScriptExecutor = driver as IJavaScriptExecutor;
                        return javaScriptExecutor != null
                               && javaScriptExecutor.ExecuteAsyncScript(
                                   "var callback = arguments[arguments.length - 1];"
                                   + "var el = document.querySelector('body');" + "if (!window.angular) "
                                   + "{callback('false')}" + "if (angular.getTestability) "
                                   + "{angular.getTestability(el).whenStable(function(){callback('true')});} " + "else "
                                   + "{if (!angular.element(el).injector())" + "{callback('false')}"
                                   + "var browser = angular.element(el).injector().get('$browser');"
                                   + "browser.notifyWhenNoOutstandingRequests(function(){callback('true')});}").Equals("true");
                    });
            }
            catch (InvalidOperationException)
            {
            }
        }


        public static void WaitForElementToBeDisplayed(this IWebDriver driver, ElementLocator locator, double timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(drv => drv.GetElement(locator).Displayed && drv.GetElement(locator).Enabled);
        }

        public static void SendKeys(this IWebDriver driver, ElementLocator locator, object text)
        {
            if (text != null && text.ToString().Equals(string.Empty))
            {
                driver.GetElement(locator).Clear();
            }
            else if(text != null)
            {
                driver.GetElement(locator).Clear();
                driver.GetElement(locator).SendKeys(text.ToString());
            }
        }

        public static void Click(this IWebDriver driver, ElementLocator locator)
        {
            driver.ScrollIntoMiddle(locator);
            driver.GetElement(locator).Click();
        }
    }
}
