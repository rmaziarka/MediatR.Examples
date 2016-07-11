namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.ActivityTabs;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewActivityPage : ProjectPageBase
    {
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator viewActivityForm = new ElementLocator(Locator.CssSelector, "activity-view > div");
        private readonly ElementLocator addressElement = new ElementLocator(Locator.XPath, "//card[@id = 'card-property']//span[text()='{0}']");
        private readonly ElementLocator propertyCard = new ElementLocator(Locator.CssSelector, ".active #card-property");
        private readonly ElementLocator editButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'goToEdit']");
        private readonly ElementLocator askingPrice = new ElementLocator(Locator.Id, "asking-price");
        private readonly ElementLocator recommendedPrice = new ElementLocator(Locator.Id, "recommendedPrice");
        private readonly ElementLocator activityStatus = new ElementLocator(Locator.Id, "activityStatus");
        private readonly ElementLocator vendorEstimatedPrice = new ElementLocator(Locator.Id, "vendorEstimatedPrice");
        private readonly ElementLocator activityTitle = new ElementLocator(Locator.CssSelector, "#activity-view-well div:nth-of-type(1)");
        private readonly ElementLocator activityType = new ElementLocator(Locator.Id, "activityType");
        private readonly ElementLocator vendor = new ElementLocator(Locator.CssSelector, ".active #activity-vendors-view list-item span");
        private readonly ElementLocator attendeeOnOverview = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item .card-item");
        private readonly ElementLocator appraisalDate = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item div[ng-transclude='header']");
        private readonly ElementLocator appraisalTime = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item .card-info");
        // Viewing locators
        private readonly ElementLocator viewings = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-group-item card[item='viewing']");
        private readonly ElementLocator viewingDetailsLink = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) .card");
        private readonly ElementLocator viewingDetails = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) .ng-binding");
        // Offer locators
        private readonly ElementLocator offers = new ElementLocator(Locator.CssSelector, ".activity-view-offers .card-body");
        private readonly ElementLocator offer = new ElementLocator(Locator.CssSelector, ".activity-view-offers:nth-of-type({0}) .card-body");
        private readonly ElementLocator offerStatus = new ElementLocator(Locator.CssSelector, ".activity-view-offers:nth-of-type({0}) .offer-status");
        private readonly ElementLocator offerData = new ElementLocator(Locator.CssSelector, ".activity-view-offers:nth-of-type({0}) .ng-binding");
        // Negotiators locators
        private readonly ElementLocator leadNegotiator = new ElementLocator(Locator.CssSelector, ".active #card-lead-negotiator .panel-item");
        private readonly ElementLocator secondaryNegotiatorData = new ElementLocator(Locator.CssSelector, "#card-list-negotiators card-list-item .ng-binding");
        private readonly ElementLocator leadNegotiatorNextCallDate = new ElementLocator(Locator.CssSelector, ".active #card-lead-negotiator + editable-date time"); 
        private readonly ElementLocator leadNegotiatorNextCallEditButton = new ElementLocator(Locator.CssSelector, ".active editable-date[selected-date *= 'leadNegotiator'] button[ng-click *= 'openEditMode']");
        private readonly ElementLocator leadNegotiatorNextCallDateField = new ElementLocator(Locator.CssSelector, ".active editable-date[selected-date *= 'leadNegotiator'] #next-call-1");
        private readonly ElementLocator leadNegotiatorNextCallSaveButton = new ElementLocator(Locator.CssSelector, ".active editable-date[selected-date *= 'leadNegotiator'] button[type = 'submit']");
        private readonly ElementLocator secondaryNegotiatorNextCallEditButton = new ElementLocator(Locator.XPath, "//div[@class='tab-pane ng-scope active']//div[text()='{0}']/ancestor::card/following-sibling::editable-date//button");
        private readonly ElementLocator secondaryNegotiatorNextCallDateField = new ElementLocator(Locator.XPath, "//div[@class='tab-pane ng-scope active']//div[text()='{0}']/ancestor::card/following-sibling::editable-date//input");
        private readonly ElementLocator secondaryNegotiatorNextCallSaveButton = new ElementLocator(Locator.XPath, "//div[@class='tab-pane ng-scope active']//div[text()='{0}']/ancestor::card/following-sibling::editable-date//button[@type='submit']");
        // Departments locators
        private readonly ElementLocator departmentName = new ElementLocator(Locator.CssSelector,"#departments-section card-list-item .card-item .department-name");
        // Property locators
        private readonly ElementLocator propertyNumber = new ElementLocator(Locator.CssSelector, ".active #activity-view-property .card-details ng-include > div:nth-of-type(1) >div:nth-of-type(1) span");
        private readonly ElementLocator propertyName = new ElementLocator(Locator.CssSelector, ".active #activity-view-property .card-details ng-include > div:nth-of-type(1) >div:nth-of-type(2) span");
        private readonly ElementLocator propertyLine2 = new ElementLocator(Locator.CssSelector, ".active #activity-view-property .card-details ng-include > div:nth-of-type(2) span");
        private readonly ElementLocator propertyPostCode = new ElementLocator(Locator.CssSelector, ".active #activity-view-property .card-details ng-include > div:nth-of-type(4) span");
        private readonly ElementLocator propertyCity = new ElementLocator(Locator.CssSelector, ".active #activity-view-property .card-details ng-include > div:nth-of-type(5) span");
        private readonly ElementLocator propertyCounty = new ElementLocator(Locator.CssSelector, ".active #activity-view-property .card-details ng-include > div:nth-of-type(6) span");
        // Details locators
        private readonly ElementLocator source = new ElementLocator(Locator.Id, "sourceId");
        private readonly ElementLocator sourceDescription = new ElementLocator(Locator.Id, "sourceDescriptionId");
        private readonly ElementLocator sellingReason = new ElementLocator(Locator.Id, "sellingReasonId");
        private readonly ElementLocator pitchingThreats = new ElementLocator(Locator.Id, "pitchingThreatsId");
        private readonly ElementLocator keyNumber = new ElementLocator(Locator.Id, "keyNumberId");
        private readonly ElementLocator accessArangements = new ElementLocator(Locator.Id, "accessArrangementsId");
        // Tabs locators
        private readonly ElementLocator detailsTab = new ElementLocator(Locator.CssSelector, "li[heading = 'Details']");
        private readonly ElementLocator overviewTab = new ElementLocator(Locator.CssSelector, "li[heading = 'Overview']");
        private readonly ElementLocator attachmentsTab = new ElementLocator(Locator.CssSelector, "li[heading = 'Attachments']");

        private const string Format = "dd-MM-yyyy";

        public ViewActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public AttachmentsTabPage AttachmentsTab => new AttachmentsTabPage(this.DriverContext);

        public PropertyPreviewPage PropertyPreview => new PropertyPreviewPage(this.DriverContext);

        public ViewingDetailsPage ViewingDetails => new ViewingDetailsPage(this.DriverContext);

        public OfferPreviewPage OfferPreview => new OfferPreviewPage(this.DriverContext);

        public string AskingPrice => this.Driver.GetElement(this.askingPrice).Text;

        public string RecommendedPrice => this.Driver.GetElement(this.recommendedPrice).Text;

        public string VendorEstimatedPrice => this.Driver.GetElement(this.vendorEstimatedPrice).Text;

        public string Status => this.Driver.GetElement(this.activityStatus).Text;

        public int ViewingsNumber => this.Driver.GetElements(this.viewings).Count;

        public string LeadNegotiator => this.Driver.GetElement(this.leadNegotiator).Text;

        public int OffersNumber => this.Driver.GetElements(this.offers).Count;

        public string LeadNegotiatorNextCall => this.Driver.GetElement(this.leadNegotiatorNextCallDate).Text;

        public List<Department> Departments
            => this.Driver.GetElements(this.departmentName).Select(el => new Department { Name = el.Text }).ToList();

        public ViewActivityPage OpenViewActivityPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("view activity", id);
            return this;
        }

        public ViewActivityPage ClickPropertyCard()
        {
            this.Driver.Click(this.propertyCard);
            return this;
        }

        public bool IsViewActivityFormPresent()
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.IsElementPresent(this.viewActivityForm, BaseConfiguration.LongTimeout);
        }

        public bool IsAddressDetailsVisible(string propertyDetail)
        {
            return this.Driver.IsElementPresent(this.addressElement.Format(propertyDetail), BaseConfiguration.MediumTimeout);
        }

        public bool IsAddressDetailsNotVisible(string propertyDetail)
        {
            return !this.Driver.IsElementPresent(this.addressElement.Format(propertyDetail), BaseConfiguration.ShortTimeout);
        }

        public ViewActivityPage EditActivity()
        {
            this.Driver.Click(this.editButton);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public ViewActivityPage OpenViewingDetails(int position)
        {
            this.Driver.Click(this.viewingDetailsLink.Format(position));
            return this;
        }

        public ViewActivityPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public ViewActivityPage WaitForSidePanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public ViewActivityPage WaitForSidePanelToHide(double timeout)
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, timeout);
            return this;
        }

        public ViewActivityPage OpenOfferDetails(int position)
        {
            this.Driver.Click(this.offer.Format(position));
            return this;
        }

        public List<string> GetOfferDetails(int position)
        {
            List<string> details = this.Driver.GetElements(this.offerData.Format(position)).Select(el => el.Text).ToList();
            details.Add(this.Driver.GetElement(this.offerStatus.Format(position)).Text);
            return details;
        }

        public List<string> GetViewingDetails(int position)
        {
            return this.Driver.GetElements(this.viewingDetails.Format(position)).Select(el => el.Text).ToList();
        }

        public ViewActivityPage EditLeadNegotiatorNextCall(int day)
        {
            this.Driver.Click(this.leadNegotiatorNextCallEditButton);
            this.Driver.SendKeys(this.leadNegotiatorNextCallDateField, DateTime.UtcNow.AddDays(day).ToString(Format));
            this.Driver.Click(this.leadNegotiatorNextCallSaveButton);
            return this;
        }

        public ViewActivityPage EditSecondaryNegotiatorNextCall(string name, int day)
        {
            this.Driver.Click(this.secondaryNegotiatorNextCallEditButton.Format(name));
            this.Driver.SendKeys(this.secondaryNegotiatorNextCallDateField.Format(name),
                DateTime.UtcNow.AddDays(day).ToString(Format));
            this.Driver.Click(this.secondaryNegotiatorNextCallSaveButton.Format(name));
            return this;
        }

        public List<Negotiator> GetSecondaryNegotiatorsData()
        {
            List<string> odds =
                this.Driver.GetElements(this.secondaryNegotiatorData)
                    .ToList()
                    .Where((c, i) => i % 2 != 0)
                    .Select(el => el.Text)
                    .ToList();
            List<string> evens =
                this.Driver.GetElements(this.secondaryNegotiatorData)
                    .ToList()
                    .Where((c, i) => i % 2 == 0)
                    .Select(el => el.Text)
                    .ToList();

            return evens.Zip(odds, (s, s1) => new Negotiator { Name = s, NextCall = s1 }).ToList();
        }

        public ViewActivityPage OpenDetailsTab()
        {
            this.Driver.Click(this.detailsTab);
            return this;
        }

        public ViewActivityPage OpenOverviewTab()
        {
            this.Driver.Click(this.overviewTab);
            return this;
        }

        public ViewActivityPage OpenAttachmentsTab()
        {
            this.Driver.Click(this.attachmentsTab);
            return this;
        }

        public Dictionary<string, string> GetActivityDetails()
        {
            var actualDetails = new Dictionary<string, string>
            {
                { "title", this.Driver.GetElement(this.activityTitle).Text },
                { "status", this.Driver.GetElement(this.activityStatus).Text },
                { "type", this.Driver.GetElement(this.activityType).Text }
            };
            return actualDetails;
        }

        public Dictionary<string, string> GetPropertyAddressOnActivityOverviewTab()
        {
            var propertyAddress = new Dictionary<string, string>
            {
                { "number", this.Driver.GetElement(this.propertyNumber).Text },
                { "name", this.Driver.GetElement(this.propertyName).Text },
                { "line2", this.Driver.GetElement(this.propertyLine2).Text },
                { "postCode", this.Driver.GetElement(this.propertyPostCode).Text },
                { "city", this.Driver.GetElement(this.propertyCity).Text },
                { "county", this.Driver.GetElement(this.propertyCounty).Text }
            };
            return propertyAddress;
        }

        public Dictionary<string, string> GetActivityDetailsOnOverviewTab()
        {
            var activityDetails = new Dictionary<string, string>
            {
                { "vendor", this.Driver.GetElement(this.vendor).Text },
                { "negotiator", this.Driver.GetElement(this.leadNegotiator).Text },
                { "attendee", this.Driver.GetElement(this.attendeeOnOverview).Text }
            };
            return activityDetails;
        }

        public Dictionary<string, string> GetAppraisalDateAndTime()
        {
            var appraisalDateTime = new Dictionary<string, string>
            {
                { "date", this.Driver.GetElement(this.appraisalDate).Text },
                { "time", this.Driver.GetElement(this.appraisalTime).Text }
            };
            return appraisalDateTime;
        }

        public Dictionary<string, string> GetActivityDetailsOnDetailsTab()
        {
            var details = new Dictionary<string, string>
            {
                { "vendor", this.Driver.GetElement(this.vendor).Text },
                { "negotiator", this.Driver.GetElement(this.leadNegotiator).Text },
                { "department", this.Driver.GetElement(this.departmentName).Text },
                { "source", this.Driver.GetElement(this.source).Text },
                { "sourceDescription", this.Driver.GetElement(this.sourceDescription).Text },
                { "sellingReason", this.Driver.GetElement(this.sellingReason).Text },
                { "pitchingThreats", this.Driver.GetElement(this.pitchingThreats).Text },
                { "keyNumber", this.Driver.GetElement(this.keyNumber).Text },
                { "accessArangements", this.Driver.GetElement(this.accessArangements).Text }
            };
            return details;
        }

        public class Attachment
        {
            public string FileName { get; set; }

            public string Type { get; set; }

            public string Size { get; set; }

            public string Date { get; set; }

            public string User { get; set; }
        }

        public class Department
        {
            public string Name { get; set; }
        }
    }
}
