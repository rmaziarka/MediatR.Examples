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
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator loadingIndicator = new ElementLocator(Locator.CssSelector, "[ng-show *= 'isLoading']");
        private readonly ElementLocator requirementDate = new ElementLocator(Locator.CssSelector, "span[translate *= 'CREATEDDATE'] ~ span");
        // Basic information
        private readonly ElementLocator requirementType = new ElementLocator(Locator.Id, "requirement-preview-type");
        private readonly ElementLocator requirementDescription = new ElementLocator(Locator.CssSelector, "requirement-description-preview-control .ng-binding");
        // Rent
        private readonly ElementLocator rent = new ElementLocator(Locator.CssSelector, ".range-value");
        // Applicants
        private readonly ElementLocator applicants = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'contacts'] div");
        // Location
        private readonly ElementLocator locationRequirements = new ElementLocator(Locator.XPath, "//*[contains(@translate, 'LOCATION')]/..//span");
        // Notes
        private readonly ElementLocator addNotes = new ElementLocator(Locator.Id, "notes-button");
        private readonly ElementLocator notesNumber = new ElementLocator(Locator.CssSelector, "#notes-button .ng-binding");
        // Viewings
        private readonly ElementLocator addViewing = new ElementLocator(Locator.CssSelector, "#viewings-list button");
        private readonly ElementLocator viewings = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item .card");
        private readonly ElementLocator viewing = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) .card-body");
        private readonly ElementLocator viewingData = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) .ng-binding");
        private readonly ElementLocator viewingActions = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) .card-menu-button");
        // Offers
        private readonly ElementLocator addOffer = new ElementLocator(Locator.CssSelector, "#viewings-list card-list-item:nth-of-type({0}) [action *= 'showAddOfferPanel']");
        private readonly ElementLocator offers = new ElementLocator(Locator.CssSelector, ".requirement-view-offers .card-body");
        private readonly ElementLocator offer = new ElementLocator(Locator.CssSelector, ".requirement-view-offers:nth-of-type({0}) .card-body");
        private readonly ElementLocator offerActions = new ElementLocator(Locator.CssSelector, ".requirement-view-offers:nth-of-type({0}) .card-menu-button");
        private readonly ElementLocator offerStatus = new ElementLocator(Locator.CssSelector, ".requirement-view-offers:nth-of-type({0}) .offer-status");
        private readonly ElementLocator offerPrice = new ElementLocator(Locator.CssSelector, ".requirement-view-offers:nth-of-type({0}) .offer-price");
        private readonly ElementLocator offerData = new ElementLocator(Locator.CssSelector, ".requirement-view-offers:nth-of-type({0}) address-form-view .ng-binding");
        private readonly ElementLocator editOffer = new ElementLocator(Locator.CssSelector, ".requirement-view-offers:nth-of-type({0}) [action *= 'showOfferEditPanel'] li");
        private readonly ElementLocator detailsOffer = new ElementLocator(Locator.CssSelector, ".requirement-view-offers:nth-of-type({0}) [action *= 'showOfferDetailsView'] li");
        // Attachments
        private readonly ElementLocator addAttachment = new ElementLocator(Locator.CssSelector, "#card-list-attachments button");
        private readonly ElementLocator attachmentFileTitle = new ElementLocator(Locator.CssSelector, "#card-list-attachments div[id *= 'attachment-data'");
        private readonly ElementLocator attachmentDate = new ElementLocator(Locator.CssSelector, "#card-list-attachments time[id *= 'attachment-created-date']");
        private readonly ElementLocator attachmentType = new ElementLocator(Locator.CssSelector, "#card-list-attachments span[id *= 'attachment-type']");
        private readonly ElementLocator attachmentSize = new ElementLocator(Locator.CssSelector, "#card-list-attachments span[id *= 'attachment-file-size']");
        private readonly ElementLocator attachmentCard = new ElementLocator(Locator.CssSelector, "#card-list-attachments .card-body");

        public ViewRequirementPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public NotesPage Notes => new NotesPage(this.DriverContext);

        public ActivityListPage ActivityList => new ActivityListPage(this.DriverContext);

        public CreateViewingPage Viewing => new CreateViewingPage(this.DriverContext);

        public ViewingDetailsPage ViewingDetails => new ViewingDetailsPage(this.DriverContext);

        public OfferPreviewPage OfferPreview => new OfferPreviewPage(this.DriverContext);

        public CreateOfferPage Offer => new CreateOfferPage(this.DriverContext);

        public int ViewingsNumber => this.Driver.GetElements(this.viewings).Count;

        public int OffersNumber => this.Driver.GetElements(this.offers).Count;

        public string CreateDate => this.Driver.GetElement(this.requirementDate).Text;

        public List<string> Applicants => this.Driver.GetElements(this.applicants).Select(el => el.Text).ToList();

        public string RequirementType => this.Driver.GetElement(this.requirementType).Text;

        public string RequirementDescription => this.Driver.GetElement(this.requirementDescription).Text;

        public string Rent => this.Driver.GetElement(this.rent).Text;

        public Attachment AttachmentDetails => new Attachment
        {
            FileName = this.Driver.GetElement(this.attachmentFileTitle).Text,
            Type = this.Driver.GetElement(this.attachmentType).Text,
            Size = this.Driver.GetElement(this.attachmentSize).Text,
            Date = this.Driver.GetElement(this.attachmentDate).Text
        };

        public AttachFilePage AttachFile => new AttachFilePage(this.DriverContext);

        public AttachmentPreviewPage AttachmentPreview => new AttachmentPreviewPage(this.DriverContext);

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
            List<string> locationDetails = this.Driver.GetElements(this.locationRequirements).Select(el => el.Text).ToList();

            List<string> keys = locationDetails.Select(el => el).Where((el, index) => index % 2 == 0).ToList();
            List<string> values = locationDetails.Select(el => el).Where((el, index) => index % 2 != 0).ToList();

            return keys.Zip(values, Tuple.Create)
                       .ToDictionary(pair => pair.Item1, pair => pair.Item2);
        }

        public ViewRequirementPage OpenNotes()
        {
            this.Driver.Click(this.addNotes);
            return this;
        }

        public string CheckNotesNumber()
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.GetElement(this.notesNumber).Text;
        }

        public ViewRequirementPage AddViewings()
        {
            this.Driver.Click(this.addViewing);
            return this;
        }

        public ViewRequirementPage OpenViewingDetails(int position)
        {
            this.Driver.Click(this.viewing.Format(position));
            return this;
        }

        public ViewRequirementPage OpenOfferDetails(int position)
        {
            this.Driver.Click(this.offer.Format(position));
            return this;
        }

        public List<string> GetViewingDetails(int position)
        {
            return this.Driver.GetElements(this.viewingData.Format(position)).Select(el => el.Text).ToList();
        }

        public List<string> GetOfferDetails(int position)
        {
            List<string> list =
                this.Driver.GetElements(this.offerData.Format(position), element => element.Enabled)
                    .Select(el => el.GetTextContent())
                    .ToList();
            string data = string.Join(" ", list).Trim();
            return new List<string>()
            {
                data,
                this.Driver.GetElement(this.offerStatus.Format(position)).Text,
                this.Driver.GetElement(this.offerPrice.Format(position)).Text
            };
        }

        public ViewRequirementPage OpenViewingActions(int position)
        {
            this.Driver.Click(this.viewingActions.Format(position));
            return this;
        }

        public ViewRequirementPage OpenOfferActions(int position)
        {
            this.Driver.Click(this.offerActions.Format(position));
            return this;
        }

        public ViewRequirementPage CreateOffer(int position)
        {
            this.Driver.Click(this.addOffer.Format(position));
            return this;
        }


        public ViewRequirementPage EditOffer(int position)
        {
            this.Driver.Click(this.editOffer.Format(position));
            return this;
        }

        public ViewRequirementPage DetailsOffer(int position)
        {
            this.Driver.Click(this.detailsOffer.Format(position));
            return this;
        }

        public ViewRequirementPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public ViewRequirementPage WaitForSidePanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public ViewRequirementPage WaitForSidePanelToHide(double timeout)
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, timeout);
            return this;
        }

        public ViewRequirementPage OpenAttachFilePanel()
        {
            this.Driver.Click(this.addAttachment);
            return this;
        }

        public ViewRequirementPage OpenAttachmentPreview()
        {
            this.Driver.Click(this.attachmentCard);
            return this;
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
