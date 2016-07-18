namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ChainTransactionPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator endOfChain = new ElementLocator(Locator.CssSelector, ".chain-status");
        private readonly ElementLocator property = new ElementLocator(Locator.CssSelector, "#offer-chain-preview-property address-form-view .ng-binding");
        private readonly ElementLocator vendor = new ElementLocator(Locator.Id, "offer-chain-preview-vendor");
        private readonly ElementLocator buyer = new ElementLocator(Locator.Id, "offer-chain-preview-buyer");
        private readonly ElementLocator knightFrankAgent = new ElementLocator(Locator.Id, "offer-chain-preview-user-agent");
        private readonly ElementLocator otherAgent = new ElementLocator(Locator.CssSelector, "#offer-chain-preview-agent-contact .ng-binding");
        private readonly ElementLocator solicitor = new ElementLocator(Locator.CssSelector, "#offer-chain-preview-solicitor .ng-binding");
        private readonly ElementLocator mortgage = new ElementLocator(Locator.Id, "offer-chain-preview-mortgage");
        private readonly ElementLocator survey = new ElementLocator(Locator.Id, "offer-chain-preview-survey");
        private readonly ElementLocator surveyDate = new ElementLocator(Locator.Id, "offer-chain-preview-survey-date");
        private readonly ElementLocator searches = new ElementLocator(Locator.Id, "offer-chain-preview-searches");
        private readonly ElementLocator enquiries = new ElementLocator(Locator.Id, "offer-chain-preview-enquiries");
        private readonly ElementLocator contractAgreed = new ElementLocator(Locator.Id, "offer-chain-preview-contract-agreed");
        private readonly ElementLocator editButton = new ElementLocator(Locator.CssSelector, "offer-chain-preview-card [ng-click *= 'edit']");

        public ChainTransactionPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string Vendor => this.Driver.GetElement(this.vendor).Text;

        public string Buyer => this.Driver.GetElement(this.buyer).Text;

        public string KnightFrankAgent => this.Driver.GetElement(this.knightFrankAgent).Text;

        public List<string> OtherAgent => this.Driver.GetElements(this.otherAgent).Select(el => el.Text).ToList();

        public List<string> Solicitor => this.Driver.GetElements(this.solicitor).Select(el => el.Text).ToList();

        public string Mortgage => this.Driver.GetElement(this.mortgage).Text;

        public string Survey => this.Driver.GetElement(this.survey).Text;

        public string SurveyDate => this.Driver.GetElement(this.surveyDate).Text;

        public string Searches => this.Driver.GetElement(this.searches).Text;

        public string Enquiries => this.Driver.GetElement(this.enquiries).Text;

        public string ContractAgreed => this.Driver.GetElement(this.contractAgreed).Text;

        public ChainTransactionPreviewPage EditChain()
        {
            this.Driver.Click(this.editButton);
            return this;
        }

        public string GetProperty()
        {
            List<string> list =
                this.Driver.GetElements(this.property, element => element.Enabled).Select(el => el.GetTextContent()).ToList();
            return string.Join(" ", list).Trim();
        }

        public bool IsEndOfChain()
        {
            return this.Driver.IsElementPresent(this.endOfChain, BaseConfiguration.ShortTimeout);
        }
    }
}
