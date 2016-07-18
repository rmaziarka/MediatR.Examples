namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateChainTransactionPage : ProjectPageBase
    {
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator endOfChain = new ElementLocator(Locator.Id, "offer-chain-edit-is-end");
        private readonly ElementLocator addProperty = new ElementLocator(Locator.Id, "property-card-edit-add");
        private readonly ElementLocator property = new ElementLocator(Locator.CssSelector, "#property-card-edit .ng-binding");
        private readonly ElementLocator vendor = new ElementLocator(Locator.Id, "offer-chain-edit-vendor");
        // KF agent
        private readonly ElementLocator knightFrankAgent = new ElementLocator(Locator.CssSelector, "#agent label:nth-of-type(1) span");
        private readonly ElementLocator searchKnighFrankAgent = new ElementLocator(Locator.CssSelector, "#user-search input");
        private readonly ElementLocator listKnightFrankAgent = new ElementLocator(Locator.XPath, "//search[@id = 'user-search']//span[contains(., '{0}')]");
        private readonly ElementLocator selectedKnightFrankAgent = new ElementLocator(Locator.CssSelector, "search-user-control .ng-binding");
        // 3rd party agent
        private readonly ElementLocator otherAgent = new ElementLocator(Locator.CssSelector, "#agent label:nth-of-type(2) span");
        private readonly ElementLocator addOtherAgent = new ElementLocator(Locator.Id, "offer-chain-edit-agent-company-contact-add-edit");
        private readonly ElementLocator selectedOtherAgent = new ElementLocator(Locator.CssSelector, "#offer-chain-edit-agent-company-contact .ng-binding");
        private readonly ElementLocator addSolicitor = new ElementLocator(Locator.Id, "offer-chain-edit-solicitor-add-edit");
        private readonly ElementLocator solicitor = new ElementLocator(Locator.CssSelector, "#offer-chain-edit-solicitor .ng-binding");
        private readonly ElementLocator mortgage = new ElementLocator(Locator.Id, "offer-chain-edit-mortgage");
        private readonly ElementLocator survey = new ElementLocator(Locator.Id, "offer-chain-edit-survey");
        private readonly ElementLocator surveyDate = new ElementLocator(Locator.Id, "offer-chain-edit-survey-date");
        private readonly ElementLocator searches = new ElementLocator(Locator.Id, "offer-chain-edit-searches");
        private readonly ElementLocator enquiries = new ElementLocator(Locator.Id, "offer-chain-edit-enquiries");
        private readonly ElementLocator contractAgreed = new ElementLocator(Locator.Id, "offer-chain-edit-contract-agreed");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "chain-add-edit-button");

        public CreateChainTransactionPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public SelectableContactsListPage ContactsList => new SelectableContactsListPage(this.DriverContext);

        public PropertiesListPage PropertiesList => new PropertiesListPage(this.DriverContext);

        public List<string> OtherAgent => this.Driver.GetElements(this.selectedOtherAgent).Select(el => el.Text).ToList();

        public List<string> Solicitor => this.Driver.GetElements(this.solicitor).Select(el => el.Text).ToList();

        public string KnightFrankAgent => this.Driver.GetElement(this.selectedKnightFrankAgent).Text;

        public CreateChainTransactionPage AddProperty()
        {
            this.Driver.Click(this.addProperty);
            return this;
        }

        public string GetProperty()
        {
            List<string> list =
                this.Driver.GetElements(this.property, element => element.Enabled).Select(el => el.GetTextContent()).ToList();
            return string.Join(" ", list).Trim();
        }

        public CreateChainTransactionPage SetVendor(string text)
        {
            this.Driver.SendKeys(this.vendor, text);
            return this;
        }

        public CreateChainTransactionPage SelectEndOfChain()
        {
            this.Driver.GetElement<Checkbox>(this.endOfChain).TickCheckbox();
            return this;
        }

        public CreateChainTransactionPage SelectKnightFrankAgent()
        {
            this.Driver.Click(this.knightFrankAgent);
            return this;
        }

        public CreateChainTransactionPage SetKnightFrankAgent(string text)
        {
            this.Driver.SendKeys(this.searchKnighFrankAgent, text);
            this.Driver.WaitForElementToBeDisplayed(this.listKnightFrankAgent.Format(text), BaseConfiguration.MediumTimeout);
            this.Driver.Click(this.listKnightFrankAgent.Format(text));
            this.Driver.WaitUntilElementIsNoLongerFound(this.listKnightFrankAgent.Format(text), BaseConfiguration.MediumTimeout);
            return this;
        }

        public CreateChainTransactionPage SelectOtherAgent()
        {
            this.Driver.Click(this.otherAgent);
            return this;
        }

        public CreateChainTransactionPage AddOtherAgent()
        {
            this.Driver.Click(this.addOtherAgent);
            return this;
        }

        public CreateChainTransactionPage AddSolicitor()
        {
            this.Driver.Click(this.addSolicitor);
            return this;
        }

        public CreateChainTransactionPage SelectMortgage(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.mortgage).SelectByText(text);
            }
            return this;
        }

        public CreateChainTransactionPage SelectSurvey(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.survey).SelectByText(text);
            }
            return this;
        }

        public CreateChainTransactionPage SetSurveyDate(string text)
        {
            this.Driver.SendKeys(this.surveyDate, text);
            return this;
        }

        public CreateChainTransactionPage SelectSearches(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.searches).SelectByText(text);
            }
            return this;
        }

        public CreateChainTransactionPage SelectEnquiries(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.enquiries).SelectByText(text);
            }
            return this;
        }

        public CreateChainTransactionPage SelectContractAgreed(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.contractAgreed).SelectByText(text);
            }
            return this;
        }

        public CreateChainTransactionPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public CreateChainTransactionPage WaitForSidePanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public CreateChainTransactionPage SaveChainTransaction()
        {
            this.Driver.Click(this.saveButton);
            this.Driver.WaitForAngularToFinish();
            return this;
        }
    }

    internal class ChainTransactionData
    {
        public string EndOfChain { get; set; }

        public string Property { get; set; }

        public string Vendor { get; set; }

        public string Buyer { get; set; }

        public string KnightFrankAgent { get; set; }

        public string OtherAgent { get; set; }

        public string OtherAgentCompany { get; set; }

        public string Solicitor { get; set; }

        public string SolicitorCompany { get; set; }

        public string Mortgage { get; set; }

        public string Survey { get; set; }

        public string SurveyDate { get; set; }

        public string Searches { get; set; }

        public string Enquiries { get; set; }

        public string ContractAgreed { get; set; }
    }
}
