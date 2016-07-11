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
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "activity-edit-save");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "activityStatusId");
        // Valuation prices
        private readonly ElementLocator askingPrice = new ElementLocator(Locator.Id, "asking-price");
        private readonly ElementLocator shortLetPricePerWeek = new ElementLocator(Locator.Id, "short-let-price-per-week");
        private readonly ElementLocator kfValuation = new ElementLocator(Locator.CssSelector, "#kfValuationPrice");
        private readonly ElementLocator dispsalType = new ElementLocator(Locator.Id, "disposalTypeId");

        private readonly ElementLocator shortKfValuationPrice = new ElementLocator(Locator.Id, "shortKfValuationPrice");
        private readonly ElementLocator longKfValuationPrice = new ElementLocator(Locator.Id, "longKfValuationPrice");
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
        // Locators for departments
        private readonly ElementLocator departmentActions = new ElementLocator(Locator.XPath, "//span[text()='{0}']/ancestor::card//a[@class = 'card-menu-button']");
        private readonly ElementLocator departmentRemove = new ElementLocator(Locator.XPath, "//span[text()='{0}']/ancestor::card//context-menu-item[@type ='remove']/li");
        private readonly ElementLocator departmentSetAsManaging = new ElementLocator(Locator.XPath, "//span[text()='{0}']/ancestor::card//context-menu-item[@type ='setAsManaging']/li");

        public EditActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public List<string> SecondaryNegotiatorsNextCalls => this.Driver.GetElements(this.secondaryNegotiatorNextCall.Format(string.Empty)).Select(el => el.GetAttribute("value")).ToList();

        public string LeadNegotiatorNextCall => this.Driver.GetElement(this.leadNegotiatorNextCall).GetAttribute("value");

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

        public EditActivityPage SetAskingPrice(string price)
        {
            this.Driver.SendKeys(this.askingPrice, price);
            return this;
        }

        public EditActivityPage SetShortLetPricePerWeek(string price)
        {
            this.Driver.SendKeys(this.shortLetPricePerWeek, price);
            return this;
        }

        public EditActivityPage SetKfValuationPricePerWeek(string price)
        {
            this.Driver.SendKeys(this.shortKfValuationPrice, price);
            this.Driver.SendKeys(this.longKfValuationPrice, price);
            return this;
        }

        public ViewActivityPage SaveActivity()
        {
            this.Driver.Click(this.saveButton);
            this.Driver.WaitForAngularToFinish();
            return new ViewActivityPage(this.DriverContext);
        }

        internal EditActivityPage SelectDisposalType(string disposalType)
        {
            this.Driver.GetElement<Select>(this.dispsalType).SelectByText(disposalType);
            return this;
        }

        public EditActivityPage SetLeadNegotiator(string leadNegotiator)
        {
            this.Driver.Click(this.editLeadNegotiator);
            this.Driver.SendKeys(this.searchLeadNegotator, leadNegotiator);
            this.Driver.WaitForElementToBeDisplayed(this.negotiator.Format(leadNegotiator), BaseConfiguration.MediumTimeout);
            this.Driver.Click(this.negotiator.Format(leadNegotiator));
            return this;
        }

        public EditActivityPage FillKfValuation(string text)
        {
            this.Driver.SendKeys(this.kfValuation, text);
            return this;
        }


        public EditActivityPage SetSecondaryNegotiator(Negotiator secondaryNegotiator)
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

        public EditActivityPage SetSecondaryNegotiatorsCallDate(int position, string date)
        {
            this.Driver.SendKeys(this.secondaryNegotiatorNextCall.Format(this.nextCall.Format(position).Value),
                date != string.Empty ? DateTime.UtcNow.AddDays(int.Parse(date)).ToString("dd-MM-yyyy") : string.Empty);
            return this;
        }

        public EditActivityPage RemoveDepartment(string departmentName)
        {
            this.Driver.Click(this.departmentActions.Format(departmentName));
            this.Driver.Click(this.departmentRemove.Format(departmentName));
            return this;
        }

        public EditActivityPage SetDepartmentAsManaging(string departmentName)
        {
            this.Driver.Click(this.departmentActions.Format(departmentName));
            this.Driver.Click(this.departmentSetAsManaging.Format(departmentName));
            return this;
        }
    }

    internal class EditActivityDetails
    {
        public string ActivityStatus { get; set; }

        public string MarketAppraisalPrice { get; set; }

        public string RecommendedPrice { get; set; }

        public string VendorEstimatedPrice { get; set; }

        public string AskingPrice { get; set; }

        public string ShortLetPricePerWeek { get; set; }

        public string KfValuationPricePerWeek { get; set; }
    }

    public class Negotiator
    {
        public string Name { get; set; }

        public string NextCall { get; set; }
    }
}
