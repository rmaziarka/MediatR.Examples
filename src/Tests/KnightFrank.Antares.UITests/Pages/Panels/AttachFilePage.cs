namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.IO;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class AttachFilePage : ProjectPageBase
    {
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "attachment-add-button");
        private readonly ElementLocator browseButton = new ElementLocator(Locator.XPath, "//span[contains(text(), 'Browse')]/input");
        private readonly ElementLocator type = new ElementLocator(Locator.CssSelector, "#document-type > select");

        public AttachFilePage(DriverContext driverContext) : base(driverContext)
        {
        }

        public AttachFilePage AddFiletoAttachment(string file)
        {
            this.Driver.GetElement(this.browseButton, element => element.Enabled)
                .SendKeys(Directory.GetCurrentDirectory() + "\\Resources\\" + file);
            return this;
        }

        public AttachFilePage SelectType(string attachmentType)
        {
            this.Driver.GetElement<Select>(this.type).SelectByText(attachmentType);
            return this;
        }

        public AttachFilePage SaveAttachment()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }
    }
}
