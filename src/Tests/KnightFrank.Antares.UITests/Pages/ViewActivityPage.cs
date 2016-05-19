namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewActivityPage : ProjectPageBase
    {
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

        public ViewActivityPage OpenViewActivityPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("view activity", id);
            return this;
        }

        public ViewActivityPage ClickDetailsLink()
        {
            this.Driver.GetElement(this.detailsLink).Click();
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
            this.Driver.GetElement(this.editButton).Click();
            this.Driver.WaitForAngularToFinish();
            return new EditActivityPage(this.DriverContext);
        }

        public ViewActivityPage OpenAttachFilePanel()
        {
            this.Driver.GetElement(this.addAttachmentButton).Click();
            return this;
        }

        public ViewActivityPage OpenAttachmentPreview()
        {
            this.Driver.GetElement(this.attachmentDetailsLink).Click();
            return this;
        }

        public ViewActivityPage OpenViewingDetails(int position)
        {
            this.Driver.GetElement(this.viewingDetailsLink.Format(position)).Click();
            return this;
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
    }

    public class Attachment
    {
        public string FileName { get; set; }

        public string Type { get; set; }

        public string Size { get; set; }

        public string Date { get; set; }

        public string User { get; set; }
    }
}
