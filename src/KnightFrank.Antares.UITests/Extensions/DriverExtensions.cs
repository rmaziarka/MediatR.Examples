using System;
using Objectivity.Test.Automation.Common;
using Objectivity.Test.Automation.Common.Extensions;
using Objectivity.Test.Automation.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KnightFrank.Antares.UITests.Extensions
{
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
                               &&
                               javaScriptExecutor.ExecuteAsyncScript(
                                   "var callback = arguments[arguments.length - 1];" +
                                   "var el = document.querySelector('html');" +
                                   "if (!window.angular) " +
                                   "{callback('false')}" +
                                   "if (angular.getTestability) " +
                                   "{angular.getTestability(el).whenStable(function(){callback('true')});} " +
                                   "else " +
                                   "{if (!angular.element(el).injector())" +
                                   "{callback('false')}" +
                                   "var browser = angular.element(el).injector().get('$browser');" +
                                   "browser.notifyWhenNoOutstandingRequests(function(){callback('true')});}").Equals("true");
                    });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public static void WaitForElementToBeDisplayed(this IWebDriver driver, By by, double timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(drv => drv.FindElement(by).Displayed && drv.FindElement(by).Enabled);
        }

        public static void WaitForElementToBeDisplayed(this IWebDriver driver, ElementLocator locator, double timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(drv => drv.GetElement(locator).Displayed && drv.GetElement(locator).Enabled);
        }
    }
}
