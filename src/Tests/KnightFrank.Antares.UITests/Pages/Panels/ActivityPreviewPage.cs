﻿namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Collections.Generic;
    using System.Linq;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ActivityPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator creationDate = new ElementLocator(Locator.Id, "activity-preview-created-date");
        private readonly ElementLocator negotiator = new ElementLocator(Locator.Id, "activity-preview-negotiator");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "activity-preview-status");
        private readonly ElementLocator type = new ElementLocator(Locator.Id, "activity-preview-type");
        private readonly ElementLocator vendors = new ElementLocator(Locator.CssSelector, "#activity-preview-vendors .ng-binding");
        private readonly ElementLocator viewActivityLink = new ElementLocator(Locator.CssSelector, ".slide-in a");

        public ActivityPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public List<string> Vendors => this.Driver.GetElements(this.vendors).Select(el => el.Text).ToList();

        public ViewActivityPage ClickViewActivity()
        {
            this.Driver.GetElement(this.viewActivityLink).Click();
            return new ViewActivityPage(this.DriverContext);
        }

        public List<string> GetActivityDetails()
        {
            var list = new List<string>
            {
                this.Driver.GetElement(this.status).Text,
                this.Driver.GetElement(this.negotiator).Text,
                this.Driver.GetElement(this.creationDate).Text,
                this.Driver.GetElement(this.type).Text
            };
            return list;
        }
    }
}
