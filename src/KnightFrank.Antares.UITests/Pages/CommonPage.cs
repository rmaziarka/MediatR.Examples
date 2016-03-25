namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Configuration;
    using System.Runtime.CompilerServices;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;

    using Xunit;

    public class CommonPage : ProjectPageBase
    {
        public CommonPage(DriverContext driverContext)
            : base(driverContext)
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
                case "new residential sales requirement":
                    this.Driver.NavigateTo(GetUrl("NewResidentialSalesRequirementPage"));
                    break;
                case "add property":
                    this.Driver.NavigateTo(GetUrl("AddPropertyPage"));
                    break;
                default:
                    Assert.True(false, "Page does not exist");
                    break;
            }
        }
    }
}
