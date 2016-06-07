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

    using OpenQA.Selenium;

    public class ViewActivityPage : ProjectPageBase
    {
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator viewActivityForm = new ElementLocator(Locator.CssSelector, "activity-view > div");
        private readonly ElementLocator addressElement = new ElementLocator(Locator.XPath, "//card[@id = 'card-property']//span[text()='{0}']");
        private readonly ElementLocator detailsLink = new ElementLocator(Locator.CssSelector, "#card-property .detailsLink");
        private readonly ElementLocator editButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'goToEdit']");
        private readonly ElementLocator marketAppraisalPrice = new ElementLocator(Locator.Id, "marketAppraisalPrice");
        private readonly ElementLocator recommendedPrice = new ElementLocator(Locator.Id, "recommendedPrice");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "activityStatus");
        private readonly ElementLocator vendorEstimatedPrice = new ElementLocator(Locator.Id, "vendorEstimatedPrice");
        // attachment locators
        private readonly ElementLocator addAttachmentButton = new ElementLocator(Locator.CssSelector, "#card-list-attachments button");
        private readonly ElementLocator attachmentFileTitle = new ElementLocator(Locator.CssSelector, "#card-list-attachments div[id *= 'attachment-data'");
        private readonly ElementLocator attachmentDate = new ElementLocator(Locator.CssSelector, "#card-list-attachments time[id *= 'attachment-created-date']");
        private readonly ElementLocator attachmentType = new ElementLocator(Locator.CssSelector, "#card-list-attachments span[id *= 'attachment-type']");
        private readonly ElementLocator attachmentSize = new ElementLocator(Locator.CssSelector, "#card-list-attachments span[id *= 'attachment-file-size']");
        private readonly ElementLocator attachmentDetailsLink = new ElementLocator(Locator.CssSelector, "#activity-view-attachments .detailsLink");
        // viewing locators
        private readonly ElementLocator viewings = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-group-item");
        private readonly ElementLocator viewingDetailsLink = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) a");
        private readonly ElementLocator viewingDetails = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) .ng-binding");
        // offer locators
        private readonly ElementLocator offers = new ElementLocator(Locator.CssSelector, ".activity-view-offers .card-body");
        private readonly ElementLocator offer = new ElementLocator(Locator.CssSelector, ".activity-view-offers:nth-of-type({0}) .card-body");
        private readonly ElementLocator offerActions = new ElementLocator(Locator.CssSelector, ".activity-view-offers:nth-of-type({0}) .card-menu-button");
        private readonly ElementLocator offerStatus = new ElementLocator(Locator.CssSelector, ".activity-view-offers:nth-of-type({0}) .offer-status");
        private readonly ElementLocator offerData = new ElementLocator(Locator.CssSelector, ".activity-view-offers:nth-of-type({0}) .ng-binding");
        // negotiators locators
        private readonly ElementLocator leadNegotiator = new ElementLocator(Locator.CssSelector, "#card-lead-negotiator .panel-item");
        private readonly ElementLocator secondaryNegotiatorData = new ElementLocator(Locator.CssSelector, "#card-list-negotiators card-list-item .ng-binding");
        private readonly ElementLocator leadNegotiatorNextCallDate = new ElementLocator(Locator.CssSelector, "#card-lead-negotiator + editable-date time"); 
        private readonly ElementLocator leadNegotiatorNextCallEditButton = new ElementLocator(Locator.CssSelector, "negotiators-view > editable-date button[ng-click *= 'openEditMode']");
        private readonly ElementLocator leadNegotiatorNextCallDateField = new ElementLocator(Locator.CssSelector, "negotiators-view > editable-date #next-call-1");
        private readonly ElementLocator leadNegotiatorNextCallSaveButton = new ElementLocator(Locator.CssSelector, "negotiators-view > editable-date button[type='submit']");
        private readonly ElementLocator secondaryNegotiatorNextCallEditButton = new ElementLocator(Locator.XPath, "//div[text()='{0}']/ancestor::card/following-sibling::editable-date//button");
        private readonly ElementLocator secondaryNegotiatorNextCallDateField = new ElementLocator(Locator.XPath, "//div[text()='{0}']/ancestor::card/following-sibling::editable-date//input");
        private readonly ElementLocator secondaryNegotiatorNextCallSaveButton = new ElementLocator(Locator.XPath, "//div[text()='{0}']/ancestor::card/following-sibling::editable-date//button[@type='submit']");
        //departments locators
        private readonly ElementLocator departmentNumber = new ElementLocator(Locator.CssSelector, "#departments-section card-list-item .card-item .department-name");
       // private readonly ElementLocator departmentName = new ElementLocator(Locator.CssSelector, "#departments-section card-list-item:nth-of-type({0}) .department-name");

        private const string Format = "dd-MM-yyyy";

        public ViewActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string MarketAppraisalPrice => this.Driver.GetElement(this.marketAppraisalPrice).Text;

        public string RecommendedPrice => this.Driver.GetElement(this.recommendedPrice).Text;

        public string VendorEstimatedPrice => this.Driver.GetElement(this.vendorEstimatedPrice).Text;

        public string Status => this.Driver.GetElement(this.status).Text;

        public PropertyPreviewPage PropertyPreview => new PropertyPreviewPage(this.DriverContext);

        public AttachFilePage AttachFile => new AttachFilePage(this.DriverContext);

        public AttachmentPreviewPage PreviewAttachment => new AttachmentPreviewPage(this.DriverContext);

        public ViewingDetailsPage ViewingDetails => new ViewingDetailsPage(this.DriverContext);

        public int ViewingsNumber => this.Driver.GetElements(this.viewings).Count;

        public string LeadNegotiator => this.Driver.GetElement(this.leadNegotiator).Text;

        public int OffersNumber => this.Driver.GetElements(this.offers).Count;

        public OfferPreviewPage OfferPreview => new OfferPreviewPage(this.DriverContext);

        public string LeadNegotiatorNextCall => this.Driver.GetElement(this.leadNegotiatorNextCallDate).Text;

        public ViewActivityPage OpenViewActivityPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("view activity", id);
            return this;
        }

        public ViewActivityPage ClickDetailsLink()
        {
            this.Driver.Click(this.detailsLink);
            return this;
        }

        public bool IsViewActivityFormPresent()
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.IsElementPresent(this.viewActivityForm, BaseConfiguration.MediumTimeout);
        }

        public bool IsAddressDetailsVisible(string propertyDetail)
        {
            return this.Driver.IsElementPresent(this.addressElement.Format(propertyDetail), BaseConfiguration.MediumTimeout);
        }

        public bool IsAddressDetailsNotVisible(string propertyDetail)
        {
            return !this.Driver.IsElementPresent(this.addressElement.Format(propertyDetail), BaseConfiguration.ShortTimeout);
        }

        public EditActivityPage EditActivity()
        {
            this.Driver.Click(this.editButton);
            this.Driver.WaitForAngularToFinish();
            return new EditActivityPage(this.DriverContext);
        }

        public ViewActivityPage OpenAttachFilePanel()
        {
            this.Driver.Click(this.addAttachmentButton);
            return this;
        }

        public ViewActivityPage OpenAttachmentPreview()
        {
            this.Driver.Click(this.attachmentDetailsLink);
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

        public ViewActivityPage OpenOfferActions(int position)
        {
            this.Driver.Click(this.offerActions.Format(position));
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

        public Attachment GetAttachmentDetails()
        {
            return new Attachment
            {
                FileName = this.Driver.GetElement(this.attachmentFileTitle).Text,
                Type = this.Driver.GetElement(this.attachmentType).Text,
                Size = this.Driver.GetElement(this.attachmentSize).Text,
                Date = this.Driver.GetElement(this.attachmentDate).Text
            };
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
            this.Driver.SendKeys(this.secondaryNegotiatorNextCallDateField.Format(name), DateTime.UtcNow.AddDays(day).ToString(Format));
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

        public List<Departments> GetDepartments()
        {
            List<IWebElement> list = this.Driver.GetElements(this.departmentNumber).ToList();
            return list.Select(el => new Departments { Department = el.Text }).ToList();
        } 
    }

    public class Attachment
    {
        public string FileName { get; set; }

        public string Type { get; set; }

        public string Size { get; set; }

        public string Date { get; set; }

        public string User { get; set; }
    }

    public class Departments
    {
        public string Department { get; set; }
    }
}
