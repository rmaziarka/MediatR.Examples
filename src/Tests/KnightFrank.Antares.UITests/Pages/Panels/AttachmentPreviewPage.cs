namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.IO;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Helpers;
    using Objectivity.Test.Automation.Common.Types;

    public class AttachmentPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator addedTime = new ElementLocator(Locator.Id, "attachment-preview-created-date");
        private readonly ElementLocator closeButton = new ElementLocator(Locator.CssSelector, ".slide-in #close-side-panel");
        private readonly ElementLocator name = new ElementLocator(Locator.CssSelector, "#attachment-preview-fileName .ng-binding");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator size = new ElementLocator(Locator.Id, "attachment-preview-size");
        private readonly ElementLocator type = new ElementLocator(Locator.Id, "attachment-preview-type");
        private readonly ElementLocator user = new ElementLocator(Locator.Id, "attachment-preview-user");

        public AttachmentPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public Attachment GetAttachmentDetails()
        {
            return new Attachment
            {
                FileName = this.Driver.GetElement(this.name).Text,
                Type = this.Driver.GetElement(this.type).Text,
                Size = this.Driver.GetElement(this.size).Text,
                Date = this.Driver.GetElement(this.addedTime).Text,
                User = this.Driver.GetElement(this.user).Text
            };
        }

        public AttachmentPreviewPage CloseAttachmentPreviewPage()
        {
            this.Driver.GetElement(this.closeButton).Click();
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public void IsAttachmentDownloaded(string fileName)
        {
            int filesNumber = FilesHelper.CountFiles(this.DriverContext.DownloadFolder, FileType.Pdf);
            this.Driver.GetElement(this.name).Click();
            FilesHelper.WaitForFileOfGivenType(FileType.Pdf, filesNumber, this.DriverContext.DownloadFolder);
            FileInfo file = FilesHelper.GetLastFile(this.DriverContext.DownloadFolder, FileType.Pdf);
        }
    }
}
