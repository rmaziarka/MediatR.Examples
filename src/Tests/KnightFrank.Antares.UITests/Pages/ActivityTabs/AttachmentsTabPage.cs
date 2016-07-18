namespace KnightFrank.Antares.UITests.Pages.ActivityTabs
{
    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class AttachmentsTabPage : ProjectPageBase
    {
        private readonly ElementLocator addAttachment = new ElementLocator(Locator.CssSelector, ".tab-pane.active #addItemBtn");
        private readonly ElementLocator attachmentTitle = new ElementLocator(Locator.CssSelector, "#card-list-attachments div[id *= 'attachment-data'");
        private readonly ElementLocator attachmentDate = new ElementLocator(Locator.CssSelector, "#card-list-attachments time[id *= 'attachment-created-date']");
        private readonly ElementLocator attachmentType = new ElementLocator(Locator.CssSelector, "#card-list-attachments span[id *= 'attachment-type']");
        private readonly ElementLocator attachmentSize = new ElementLocator(Locator.CssSelector, "#card-list-attachments span[id *= 'attachment-file-size']");
        private readonly ElementLocator attachmentCard = new ElementLocator(Locator.CssSelector, "#card-list-attachments .card-body");

        public AttachmentsTabPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public AttachFilePage AttachFile => new AttachFilePage(this.DriverContext);

        public AttachmentPreviewPage AttachmentPreview => new AttachmentPreviewPage(this.DriverContext);

        public ViewActivityPage.Attachment AttachmentDetails => new ViewActivityPage.Attachment
        {
            FileName = this.Driver.GetElement(this.attachmentTitle).Text,
            Type = this.Driver.GetElement(this.attachmentType).Text,
            Size = this.Driver.GetElement(this.attachmentSize).Text,
            Date = this.Driver.GetElement(this.attachmentDate).Text
        };

        public AttachmentsTabPage OpenAttachFilePanel()
        {
            this.Driver.Click(this.addAttachment);
            return this;
        }

        public AttachmentsTabPage OpenAttachmentPreview()
        {
            this.Driver.Click(this.attachmentCard);
            return this;
        }
    }
}
