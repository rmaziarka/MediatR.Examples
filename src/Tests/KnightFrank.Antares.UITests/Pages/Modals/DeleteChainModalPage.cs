namespace KnightFrank.Antares.UITests.Pages.Modals
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class DeleteChainModalPage : ProjectPageBase
    {
        private readonly ElementLocator modalDialog = new ElementLocator(Locator.CssSelector, ".modal-dialog");
        private readonly ElementLocator modalConfirm = new ElementLocator(Locator.CssSelector, ".modal-dialog [ng-click *= 'confirmModal']");

        public DeleteChainModalPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public DeleteChainModalPage WaitForModalToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.modalDialog, BaseConfiguration.MediumTimeout);
            return this;
        }

        public DeleteChainModalPage WaitForModalToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.modalDialog, BaseConfiguration.MediumTimeout);
            return this;
        }

        public DeleteChainModalPage ConfirmModal()
        {
            this.Driver.Click(this.modalConfirm);
            this.Driver.WaitForAngularToFinish();
            return this;
        }
    }
}
