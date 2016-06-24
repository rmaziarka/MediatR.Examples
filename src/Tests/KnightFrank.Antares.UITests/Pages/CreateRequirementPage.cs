namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateRequirementPage : ProjectPageBase
    {
        private readonly ElementLocator requirementForm = new ElementLocator(Locator.Id, "addRequirementForm");
        private readonly ElementLocator saveResidentialSalesRequirement = new ElementLocator(Locator.Id, "saveBtn");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        // Applicants
        private readonly ElementLocator newApplicantButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showContactList']");
        private readonly ElementLocator applicantsList = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'requirement.contacts']");
        // Basic information
        private readonly ElementLocator requirementType = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator requirementDescription = new ElementLocator(Locator.Id, "description");
        // Rent requirements
        private readonly ElementLocator rentWeeklyMin = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator rentWeeklyMax = new ElementLocator(Locator.Id, string.Empty);

        public CreateRequirementPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public AddressTemplatePage AddressTemplate => new AddressTemplatePage(this.DriverContext);

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public List<string> Applicants => this.Driver.GetElements(this.applicantsList).Select(el => el.Text).ToList();

        public CreateRequirementPage OpenCreateRequirementPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("create requirement");
            return this;
        }

        public CreateRequirementPage SelectRequirementType(string text)
        {
            this.Driver.GetElement<Select>(this.requirementType).SelectByText(text);
            return this;
        }

        public CreateRequirementPage SetRequirementDescription(string text)
        {
            this.Driver.SendKeys(this.requirementDescription, text);
            return this;
        }

        public CreateRequirementPage SetRentWeeklyMin(string text)
        {
            this.Driver.SendKeys(this.rentWeeklyMin, text);
            return this;
        }

        public CreateRequirementPage SetRentWeeklyMax(string text)
        {
            this.Driver.SendKeys(this.rentWeeklyMax, text);
            return this;
        }

        public CreateRequirementPage SaveRequirement()
        {
            this.Driver.Click(this.saveResidentialSalesRequirement);
            this.Driver.WaitForAngularToFinish(BaseConfiguration.MediumTimeout);
            return this;
        }

        public CreateRequirementPage SelectApplicants()
        {
            this.Driver.Click(this.newApplicantButton);
            return this;
        }

        public bool IsRequirementFormPresent()
        {
            return this.Driver.IsElementPresent(this.requirementForm, BaseConfiguration.MediumTimeout);
        }

        public CreateRequirementPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public CreateRequirementPage WaitForSidePanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
