namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewRequirementPage : ProjectPageBase
    {
        private readonly ElementLocator viewRequirementForm = new ElementLocator(Locator.CssSelector, "requirement-view > div");
        private readonly ElementLocator loadingIndicator = new ElementLocator(Locator.CssSelector, "[ng-show *= 'isLoading']");
        private readonly ElementLocator locationRequirementsDetails = new ElementLocator(Locator.XPath, "//*[contains(@translate, 'LOCATION')]/..//span");
        private readonly ElementLocator notesButton = new ElementLocator(Locator.Id, "notes-button");
        private readonly ElementLocator notesNumber = new ElementLocator(Locator.CssSelector, "#notes-button .ng-binding");
        private readonly ElementLocator propertyRequirementsDetails = new ElementLocator(Locator.XPath, "//*[contains(@translate, 'BASIC_REQUIREMENTS')]/..//div[contains(@class, 'ng-binding')]");
        private readonly ElementLocator propertyRequirementsDetailsDescription = new ElementLocator(Locator.XPath, "//*[contains(@translate, 'DESCRIPTION')]/../p");
        private readonly ElementLocator requirementApplicants = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'contacts'] div");
        private readonly ElementLocator requirementDate = new ElementLocator(Locator.CssSelector, "span[translate *= 'CREATEDDATE'] ~ span");
        private readonly ElementLocator addViewings = new ElementLocator(Locator.CssSelector, "#viewings-list button");
        private readonly ElementLocator viewings = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item .panel-body");
        private readonly ElementLocator viewingDetailsLink = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) a");
        private readonly ElementLocator viewingDetails = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) .ng-binding");
        
        public ViewRequirementPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public NotesPage Notes => new NotesPage(this.DriverContext);

        public ActivityListPage ActivityList => new ActivityListPage(this.DriverContext);

        public CreateViewingPage Viewing => new CreateViewingPage(this.DriverContext);

        public ViewingDetailsPage ViewingDetails => new ViewingDetailsPage(this.DriverContext);

        public int ViewingsNumber => this.Driver.GetElements(this.viewings).Count;

        public ViewRequirementPage OpenViewRequirementPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("view requirement", id);
            return this;
        }

        public bool IsViewRequirementFormPresent()
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.IsElementPresent(this.viewRequirementForm, BaseConfiguration.MediumTimeout);
        }

        public ViewRequirementPage WaitForDetailsToLoad()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.loadingIndicator, BaseConfiguration.MediumTimeout);
            return this;
        }

        public Dictionary<string, string> GetLocationRequirements()
        {
            List<string> locationDetails = this.Driver.GetElements(this.locationRequirementsDetails).Select(el => el.Text).ToList();

            List<string> keys = locationDetails.Select(el => el).Where((el, index) => index % 2 == 0).ToList();
            List<string> values = locationDetails.Select(el => el).Where((el, index) => index % 2 != 0).ToList();

            return keys.Zip(values, Tuple.Create)
                       .ToDictionary(pair => pair.Item1, pair => pair.Item2);
        }

        public Dictionary<string, string> GetPropertyRequirements()
        {
            List<string> propertyDetails = this.Driver.GetElements(this.propertyRequirementsDetails).Select(el => el.Text).ToList();

            List<string> keys = propertyDetails.Select(el => el).Where((el, index) => index % 2 == 0).ToList();
            List<string> values = propertyDetails.Select(el => el).Where((el, index) => index % 2 != 0).ToList();

            Dictionary<string, string> details =
                keys.Zip(values, Tuple.Create)
                    .ToDictionary(pair => pair.Item1, pair => pair.Item2);
            details.Add("Requirement description", this.Driver.GetElement(this.propertyRequirementsDetailsDescription).Text);

            return details;
        }

        public string GetRequirementCreateDate()
        {
            return this.Driver.GetElement(this.requirementDate).Text;
        }

        public List<string> GetApplicants()
        {
            return this.Driver.GetElements(this.requirementApplicants).Select(el => el.Text).ToList();
        }

        public ViewRequirementPage OpenNotes()
        {
            this.Driver.GetElement(this.notesButton).Click();
            return this;
        }

        public string CheckNotesNumber()
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.GetElement(this.notesNumber).Text;
        }

        public ViewRequirementPage AddViewings()
        {
            this.Driver.GetElement(this.addViewings).Click();
            return this;
        }

        public ViewRequirementPage OpenViewingDetails(int position)
        {
            this.Driver.GetElement(this.viewingDetailsLink.Format(position)).Click();
            return this;
        }

        public List<string> GetViewingDetails(int position)
        {
            return this.Driver.GetElements(this.viewingDetails.Format(position)).Select(el => el.Text).ToList();
        }
    }

    internal class ViewingDetails
    {
        public string Name { get; set; }

        public string Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Negotiator { get; set; }

        public string Attendees { get; set; }

        public string InvitationText { get; set; }

        public string PostViewingComment { get; set; }
    }

    internal class ViewingData
    {
        public string Date { get; set; }

        public string Time { get; set; }

        public string Name { get; set; }
    }
}
