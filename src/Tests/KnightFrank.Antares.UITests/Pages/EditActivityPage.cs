namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class EditActivityPage : ProjectPageBase
    {
        private readonly ElementLocator marketAppraisalPrice = new ElementLocator(Locator.Id, "market-appraisal-price");
        private readonly ElementLocator recommendedPrice = new ElementLocator(Locator.Id, "recommended-price");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "activity-edit-save");
        private readonly ElementLocator status = new ElementLocator(Locator.CssSelector, "#activityStatus > select");
        private readonly ElementLocator vendorEstimatedPrice = new ElementLocator(Locator.Id, "vendor-estimated-price");
        // Locators for negotiators
        private readonly ElementLocator editLeadNegotiator = new ElementLocator(Locator.Id, "lead-edit-btn");
        private readonly ElementLocator searchLeadNegotator = new ElementLocator(Locator.CssSelector, "#lead-search input");
        private readonly ElementLocator addSecondaryNegotiator = new ElementLocator(Locator.CssSelector, "#card-list-negotiators button:not([ng-click *= 'cancel'])");
        private readonly ElementLocator searchSecondaryNegotiator = new ElementLocator(Locator.CssSelector, "#secondary-search input");
        private readonly ElementLocator cancelSecondaryNegotiator = new ElementLocator(Locator.CssSelector, "#card-list-negotiators [ng-click *= 'cancel']");
        private readonly ElementLocator secondaryNegotiatorActions = new ElementLocator(Locator.CssSelector, "#activity-edit-negotiators card-list-item:nth-of-type({0}) .card-menu-button");
        private readonly ElementLocator deleteSecondaryNegotiator = new ElementLocator(Locator.CssSelector, "#activity-edit-negotiators card-list-item:nth-of-type({0}) [action *= 'deleteSecondaryNegotiator']");
        private readonly ElementLocator setSecondaryNegotiatorAsLead = new ElementLocator(Locator.CssSelector, "#activity-edit-negotiators card-list-item:nth-of-type({0}) [action *= 'switchToLeadNegotiator']");
        private readonly ElementLocator negotiator = new ElementLocator(Locator.XPath, "//section[@id = 'activity-edit-negotiators']//span[contains(., '{0}')]");
        private readonly ElementLocator leadNegotiatorNextCall = new ElementLocator(Locator.Id, "lead-call-date");
        private readonly ElementLocator secondaryNegotiatorNextCall = new ElementLocator(Locator.CssSelector, "#card-list-negotiators card-list-item{0} input");
        private readonly ElementLocator nextCall = new ElementLocator(Locator.CssSelector, ":nth-of-type({0})");
        //Locators for departments
        private readonly ElementLocator departmentActions = new ElementLocator(Locator.XPath, "//span[text()='{0}']/ancestor::card//i[@class='fa fa-ellipsis-h']");
        private readonly ElementLocator departmentRemove = new ElementLocator(Locator.XPath, "//span[text()='{0}']/ancestor::card//span[text()='Remove']");
        private readonly ElementLocator departmentSetAsManaging = new ElementLocator(Locator.XPath, "//span[text()='{0}']/ancestor::card//span[text()='Set as managing']");

        public EditActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public List<string> SecondaryNegotiatorsNextCalls => this.Driver.GetElements(this.secondaryNegotiatorNextCall.Format(string.Empty)).Select(el => el.GetAttribute("value")).ToList();

        public EditActivityPage OpenEditActivityPage(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("edit activity", id);
            return this;
        }

        public EditActivityPage SelectActivityStatus(string newStatus)
        {
            this.Driver.GetElement<Select>(this.status).SelectByText(newStatus);
            return this;
        }

        public EditActivityPage SetMarketAppraisalPrice(int price)
        {
            this.Driver.SendKeys(this.marketAppraisalPrice, price.ToString());
            return this;
        }

        public EditActivityPage SetRecommendedPrice(int price)
        {
            this.Driver.SendKeys(this.recommendedPrice, price.ToString());
            return this;
        }

        public EditActivityPage SetVendorEstimatedPrice(int price)
        {
            this.Driver.SendKeys(this.vendorEstimatedPrice, price.ToString());
            return this;
        }

        public ViewActivityPage SaveActivity()
        {
            this.Driver.Click(this.saveButton);
            this.Driver.WaitForAngularToFinish();
            return new ViewActivityPage(this.DriverContext);
        }

        public EditActivityPage EditLeadNegotiator(string leadNegotiator)
        {
            this.Driver.Click(this.editLeadNegotiator);
            this.Driver.SendKeys(this.searchLeadNegotator, leadNegotiator);
            this.Driver.WaitForElementToBeDisplayed(this.negotiator.Format(leadNegotiator), BaseConfiguration.MediumTimeout);
            this.Driver.Click(this.negotiator.Format(leadNegotiator));
            return this;
        }

        public EditActivityPage AddSecondaryNegotiator(Negotiator secondaryNegotiator)
        {
            this.Driver.Click(this.addSecondaryNegotiator);
            this.Driver.SendKeys(this.searchSecondaryNegotiator, secondaryNegotiator.Name);
            this.Driver.WaitForElementToBeDisplayed(this.negotiator.Format(secondaryNegotiator.Name),
                BaseConfiguration.MediumTimeout);
            this.Driver.Click(this.negotiator.Format(secondaryNegotiator.Name));
            this.Driver.Click(this.cancelSecondaryNegotiator);
            return this;
        }

        public EditActivityPage RemoveSecondaryNegotiator(int position)
        {
            this.Driver.Click(this.secondaryNegotiatorActions.Format(position));
            this.Driver.Click(this.deleteSecondaryNegotiator.Format(position));
            return this;
        }

        public EditActivityPage SetSecondaryNegotiatorAsLeadNegotiator(int position)
        {
            this.Driver.Click(this.secondaryNegotiatorActions.Format(position));
            this.Driver.Click(this.setSecondaryNegotiatorAsLead.Format(position));
            return this;
        }

        public string GetLeadNegotiatorNextCall()
        {
            return this.Driver.GetElement(this.leadNegotiatorNextCall).GetAttribute("value");
        }

        public EditActivityPage EditSecondaryNegotiatorsCallDate(int position, string date)
        {
            this.Driver.SendKeys(this.secondaryNegotiatorNextCall.Format(this.nextCall.Format(position).Value),
                date != string.Empty ? DateTime.UtcNow.AddDays(int.Parse(date)).ToString("dd-MM-yyyy") : string.Empty);
            return this;
        }

        public EditActivityPage RemoveDepartment(string department)
        {
            this.Driver.Click(this.departmentActions.Format(department));
            this.Driver.Click(this.departmentRemove.Format(department));
            return this;
        }

        public EditActivityPage SetDepartmentAsLead(string department)
        {
            this.Driver.Click(this.departmentActions.Format(department));
            this.Driver.Click(this.departmentSetAsManaging.Format(department));
            return this;
        }
    }

    internal class EditActivityDetails
    {
        public string ActivityStatus { get; set; }

        public int MarketAppraisalPrice { get; set; }

        public int RecommendedPrice { get; set; }

        public int VendorEstimatedPrice { get; set; }
    }

    public class Negotiator
    {
        public string Name { get; set; }

        public string NextCall { get; set; }
    }
}
