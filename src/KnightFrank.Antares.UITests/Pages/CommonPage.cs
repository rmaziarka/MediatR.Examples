using System;
using System.Configuration;

using Objectivity.Test.Automation.Common;
using Objectivity.Test.Automation.Common.Extensions;

using Xunit;

namespace KnightFrank.Antares.UITests.Pages
{
    public class CommonPage : ProjectPageBase
    {
        public CommonPage(DriverContext driverContext) : base(driverContext)
        {
        }

        private static Uri GetUrl()
        {
            return new Uri(ConfigurationManager.AppSettings["protocol"] + "://" + ConfigurationManager.AppSettings["host"]);
        }

        private static Uri GetUrl(string key)
        {
            return new Uri(GetUrl() + ConfigurationManager.AppSettings[key]);
        }

        public void NavigateToPage(string page)
        {
            switch (page.ToLower())
            {
                case "home":
                    this.Driver.NavigateTo(GetUrl());
                    break;
                case "new contact":
                    this.Driver.NavigateTo(GetUrl("NewContactPage"));
                    break;
                default:
                    Assert.True(false, "Page does not exist");
                    break;
            }
        }
    }
}
