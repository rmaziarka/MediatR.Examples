namespace KnightFrank.Antares.UITests.Steps.ActivityTabs
{
    using System;
    using System.IO;

    using FluentAssertions;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class AttachmentsTabSteps
    {
        private const string Format = "dd-MM-yyyy";
        private readonly DriverContext driverContext;
        private readonly ViewActivityPage page;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;

        public AttachmentsTabSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new ViewActivityPage(this.driverContext);
            }
        }

        [When(@"User clicks add attachment button on attachments tab on view activity page")]
        public void OpenAttachFilePanel()
        {
            this.page.AttachmentsTab.OpenAttachFilePanel();
            this.page.WaitForSidePanelToShow();
        }

        [When(@"User adds (.*) file with (.*) type on attachments tab on view activity page")]
        public void AddAttachment(string file, string type)
        {
            this.page.AttachmentsTab.AttachFile.SelectType(type)
                .AddFiletoAttachment(file)
                .SaveAttachment();
            this.page.WaitForSidePanelToHide(60);
        }

        [When(@"User clicks attachment card on attachments tab on view activity page")]
        public void OpenAttachmentPreview()
        {
            this.page.AttachmentsTab.OpenAttachmentPreview();
            this.page.WaitForSidePanelToShow();
        }

        [Then(@"Activity attachment (.*) should be downloaded")]
        public void ThenAttachmentShouldBeDownloaded(string attachmentName)
        {
            FileInfo fileInfo = this.page.AttachmentsTab.AttachmentPreview.GetDownloadedAttachmentInfo();

            Verify.That(this.driverContext,
                () => Assert.Equal(attachmentName.ToLower(), fileInfo.Name),
                () => Assert.Equal("." + attachmentName.Split('.')[1], fileInfo.Extension));
        }

        [Then(@"Attachment should be displayed on attachments tab on view activity page")]
        public void CheckIfAttachmentIsDisplayed(Table table)
        {
            ViewActivityPage.Attachment actual = this.page.AttachmentsTab.AttachmentDetails;
            var expected = table.CreateInstance<ViewActivityPage.Attachment>();
            expected.Date = DateTime.UtcNow.ToString(Format);

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"Attachment preview details are the same like on attachments tab on view activity page")]
        public void ChackAttachmentDetails()
        {
            ViewActivityPage.Attachment actual = this.page.AttachmentsTab.AttachmentPreview.AttachmentDetails;
            actual.Date = actual.Date.Split(',')[0];
            ViewActivityPage.Attachment expected = this.page.AttachmentsTab.AttachmentDetails;
            expected.User = "John Smith";

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"User closes attachment preview page on attachments tab on view activity page")]
        public void CloseAttachmentPreviewPanel()
        {
            this.page.AttachmentsTab.AttachmentPreview.CloseAttachmentPreview();
            this.page.WaitForSidePanelToHide();
        }
    }
}
