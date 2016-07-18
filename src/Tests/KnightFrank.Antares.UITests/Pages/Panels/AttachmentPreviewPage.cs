namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Collections.Generic;
    using System.IO;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Helpers;
    using Objectivity.Test.Automation.Common.Types;

    using Attachment = KnightFrank.Antares.UITests.Pages.ViewActivityPage.Attachment;

    public class AttachmentPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator addedTime = new ElementLocator(Locator.Id, "attachment-preview-created-date");
        private readonly ElementLocator closeButton = new ElementLocator(Locator.CssSelector, ".slide-in #close-side-panel");
        private readonly ElementLocator name = new ElementLocator(Locator.CssSelector, "#attachment-preview-fileName .ng-binding");
        private readonly ElementLocator size = new ElementLocator(Locator.Id, "attachment-preview-size");
        private readonly ElementLocator type = new ElementLocator(Locator.Id, "attachment-preview-type");
        private readonly ElementLocator user = new ElementLocator(Locator.Id, "attachment-preview-user");

        public AttachmentPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public Attachment AttachmentDetails => new Attachment
        {
            FileName = this.Driver.GetElement(this.name).Text,
            Type = this.Driver.GetElement(this.type).Text,
            Size = this.Driver.GetElement(this.size).Text,
            Date = this.Driver.GetElement(this.addedTime).Text,
            User = this.Driver.GetElement(this.user).Text
        };

        public AttachmentPreviewPage CloseAttachmentPreview()
        {
            this.Driver.Click(this.closeButton);
            return this;
        }

        public FileInfo GetDownloadedAttachmentInfo()
        {
            ICollection<FileInfo> test = FilesHelper.GetAllFiles(this.DriverContext.DownloadFolder, ".pdf");
            foreach (FileInfo fileInfo in test)
            {
                fileInfo.Delete();
            }
            
            int filesNumber = FilesHelper.CountFiles(this.DriverContext.DownloadFolder, FileType.Pdf);
            this.Driver.Click(this.name);
            FilesHelper.WaitForFileOfGivenType(FileType.Pdf, filesNumber, this.DriverContext.DownloadFolder);
            return FilesHelper.GetLastFile(this.DriverContext.DownloadFolder, FileType.Pdf);
        }
    }
}
