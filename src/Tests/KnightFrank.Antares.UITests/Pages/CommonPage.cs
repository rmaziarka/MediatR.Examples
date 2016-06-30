namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Configuration;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;

    using Xunit;

    public class CommonPage : ProjectPageBase
    {
        public CommonPage(DriverContext driverContext) : base(driverContext)
        {
        }

        private static Uri GetUrl()
        {
            return new Uri(ConfigurationManager.AppSettings["protocol"] + "://" + ConfigurationManager.AppSettings["host"]);
        }

        public static Uri GetUrl(string key)
        {
            return new Uri(GetUrl() + ConfigurationManager.AppSettings[key]);
        }

        private static Uri GetUrl(string key, string id)
        {
            return new Uri(GetUrl() + ConfigurationManager.AppSettings[key].Replace("id", id));
        }

        public void NavigateToPage(string page)
        {
            switch (page.ToLower())
            {
                case "home":
                    this.Driver.NavigateTo(GetUrl());
                    break;
                case "create contact":
                    this.Driver.NavigateTo(GetUrl("CreateContactPage"));
                    break;
                case "create requirement":
                    this.Driver.NavigateTo(GetUrl("CreateRequirementPage"));
                    break;
                case "create property":
                    this.Driver.NavigateTo(GetUrl("CreatePropertyPage"));
                    break;
                case "create company":
                    this.Driver.NavigateTo(GetUrl("CreateCompanyPage"));
                    break;
                case "search property":
                    this.Driver.NavigateTo(GetUrl("SearchPropertyPage"));
                    break;
                case "preferences":
                    this.Driver.NavigateTo(GetUrl("PreferencesPage"));
                    break;
                default:
                    Assert.True(false, "Page does not exist");
                    break;
            }
        }

        public void NavigateToPageWithId(string page, string id)
        {
            switch (page.ToLower())
            {
                case "view requirement":
                    this.Driver.NavigateTo(GetUrl("ViewRequirementPage", id));
                    break;
                case "view property":
                    this.Driver.NavigateTo(GetUrl("ViewPropertyPage", id));
                    break;
                case "edit property":
                    this.Driver.NavigateTo(GetUrl("EditPropertyPage", id));
                    break;
                case "view activity":
                    this.Driver.NavigateTo(GetUrl("ViewActivityPage", id));
                    break;
                case "edit activity":
                    this.Driver.NavigateTo(GetUrl("EditActivityPage", id));
                    break;
                case "view offer":
                    this.Driver.NavigateTo(GetUrl("ViewOfferPage", id));
                    break;
                case "edit offer":
                    this.Driver.NavigateTo(GetUrl("EditOfferPage", id));
                    break;
                case "view company":
                    this.Driver.NavigateTo(GetUrl("ViewCompanyPage", id));
                    break;
                case "edit company":
                    this.Driver.NavigateTo(GetUrl("EditCompanyPage", id));
                    break;
                default:
                    Assert.True(false, "Page does not exist");
                    break;
            }
        }

        public void GoBack()
        {
            this.Driver.Navigate().Back();
        }
    }
}
