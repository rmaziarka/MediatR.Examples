using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool IsEditContactFormPresent()
        {
            return this.Driver.IsElementPresent(this.editContactForm, BaseConfiguration.MediumTimeout);
        }
    }
}
