namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class EditContactPage : ProjectPageBase
    {
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "saveBtn");
        private readonly ElementLocator editContactForm = new ElementLocator(Locator.CssSelector, "contact-edit > div");
        // Contact
        private readonly ElementLocator firstName = new ElementLocator(Locator.Id, "first-name");
        private readonly ElementLocator lastName = new ElementLocator(Locator.Id, "last-name");
        private readonly ElementLocator title = new ElementLocator(Locator.Id, "title");
        private readonly ElementLocator titleDropdown = new ElementLocator(Locator.XPath, "(//ul[@class='dropdown-menu ng-isolate-scope']//span[starts-with(., '{0}')])[1]");
        // Primary Negotiators
        private readonly ElementLocator primaryNegotiatorAddButton = new ElementLocator(Locator.Id, "lead-edit-btn");
        private readonly ElementLocator primaryNegotiatorSearch = new ElementLocator(Locator.CssSelector, "#lead-search input");
        private readonly ElementLocator primaryNegotiatorDropdown = new ElementLocator(Locator.XPath, "//search[@id='lead-search']//span[starts-with(., '{0}')]");
        // Secondary Negotiators
        private readonly ElementLocator secondaryNegotiatorAddButton = new ElementLocator(Locator.Id, "addItemBtn");
        private readonly ElementLocator secondaryNegotiatorSearch = new ElementLocator(Locator.CssSelector, "#secondary-search input");
        private readonly ElementLocator secondaryNegotiatorDropdown = new ElementLocator(Locator.XPath, "//search[@id='secondary-search']//span[starts-with(., '{0}')]");
        private readonly ElementLocator secondaryNegotiatorCloseButton = new ElementLocator(Locator.CssSelector, "#secondary-search button");
        private readonly ElementLocator secondaryNegotiatorActions = new ElementLocator(Locator.CssSelector, "#contact-edit-negotiators card-list-item:nth-of-type({0}) .card-menu-button");
        private readonly ElementLocator setSecondaryNegotiatorAsLead = new ElementLocator(Locator.CssSelector, "#contact-edit-negotiators card-list-item:nth-of-type({0}) [action *= 'switchToLeadNegotiator']");
        private readonly ElementLocator deleteSecondaryNegotiator = new ElementLocator(Locator.CssSelector, "#contact-edit-negotiators card-list-item:nth-of-type({0}) [action *= 'deleteSecondaryNegotiator']");

        public EditContactPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public EditContactPage SetTitle(string text)
        {
            this.Driver.SendKeys(this.title, text);
            this.Driver.WaitForAngularToFinish();
            this.Driver.WaitForElementToBeDisplayed(this.titleDropdown.Format(text), BaseConfiguration.MediumTimeout);
            this.Driver.Click(this.titleDropdown.Format(text));
            return this;
        }

        public EditContactPage SetFirstName(string text)
        {
            this.Driver.SendKeys(this.firstName, text);
            return this;
        }

        public EditContactPage SetLastName(string text)
        {
            this.Driver.SendKeys(this.lastName, text);
            return this;
        }

        public EditContactPage SaveEditedContact()
        {
            this.Driver.Click(this.saveButton);
            return this;
        }

        public EditContactPage SetPrimaryNegotiatorName(string text)
        {
            this.Driver.Click(this.primaryNegotiatorAddButton);
            this.Driver.WaitForAngularToFinish();
            this.Driver.SendKeys(this.primaryNegotiatorSearch, text);
            this.Driver.WaitForAngularToFinish();
            this.Driver.WaitForElementToBeDisplayed(this.primaryNegotiatorDropdown.Format(text), BaseConfiguration.MediumTimeout);
            this.Driver.Click(this.primaryNegotiatorDropdown.Format(text));
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public EditContactPage SetSecondaryNegotiatorName(string text)
        {
            this.Driver.Click(this.secondaryNegotiatorAddButton);
            this.Driver.WaitForAngularToFinish();
            this.Driver.SendKeys(this.secondaryNegotiatorSearch, text);
            this.Driver.WaitForAngularToFinish();
            this.Driver.WaitForElementToBeDisplayed(this.secondaryNegotiatorDropdown.Format(text), BaseConfiguration.MediumTimeout);
            this.Driver.Click(this.secondaryNegotiatorDropdown.Format(text));
            this.Driver.Click(this.secondaryNegotiatorCloseButton);
            this.Driver.WaitForAngularToFinish();
            return this;
        }
    
        public EditContactPage SetSecondaryNegotiatorAsLeadNegotiator(int position)
        {
            this.Driver.Click(this.secondaryNegotiatorActions.Format(position));
            this.Driver.Click(this.setSecondaryNegotiatorAsLead.Format(position));
            return this;
        }

        public EditContactPage RemoveSecondaryNegotiator(int position)
        {
            this.Driver.Click(this.secondaryNegotiatorActions.Format(position));
            this.Driver.Click(this.deleteSecondaryNegotiator.Format(position));
            return this;
        }

        public bool IsEditContactFormPresent()
        {
            return this.Driver.IsElementPresent(this.editContactForm, BaseConfiguration.LongTimeout);
        }
    }
}
