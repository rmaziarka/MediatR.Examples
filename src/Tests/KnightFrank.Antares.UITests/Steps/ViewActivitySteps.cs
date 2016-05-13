namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.IO;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewActivitySteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

        public ViewActivitySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [When(@"User clicks property details link on view activity page")]
        public void OpenPreviewPropertyPage()
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").ClickDetailsLink();
        }

        [When(@"User clicks view property link on property preview page")]
        public void OpenViewPropertyPage()
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");
            this.scenarioContext.Set(page.PropertyPreview.WaitForPanelToBeVisible().OpenViewPropertyPage(), "ViewPropertyPage");
        }

        [When(@"User clicks edit button on view activity page")]
        public void EditActivity()
        {
            EditActivityPage page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").EditActivity();
            this.scenarioContext.Set(page, "EditActivityPage");
        }

        [When(@"User clicks add attachment button on view activity page")]
        public void OpenAttachFilePanel()
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").OpenAttachFilePanel();
        }

        [When(@"User adds (.*) file with (.*) type on attach file panel")]
        public void SelectAttachmentType(string file, string type)
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage")
                .AttachFile.SelectType(type)
                .AddFiletoAttachment(file)
                .SaveAttachment();
        }

        [When(@"User clicks attachment details link on view activity page")]
        public void OpenAttachmentPreview()
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").OpenAttachmentPreview();
        }

        [When(@"User clicks close button on attachment preview page")]
        public void CloseAttachmentPreviewPanel()
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").PreviewAttachment.CloseAttachmentPreviewPage();
        }

        [Then(@"Attachment (.*) should be downloaded")]
        public void ThenAttachmentShouldBeDownloaded(string attachmentName)
        {
            FileInfo fileInfo =
                this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").PreviewAttachment.GetDownloadedAttachmentInfo();

            Verify.That(this.driverContext,
                () => Assert.Equal(attachmentName.ToLower(), fileInfo.Name),
                () => Assert.Equal("." + attachmentName.Split('.')[1], fileInfo.Extension));
        }

        [Then(@"Address details on view activity page are following")]
        public void CheckViewActivityAddressDetails(Table table)
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");

            foreach (string field in table.Rows.SelectMany(row => row.Values))
            {
                Assert.True(field.Equals(string.Empty)
                    ? page.IsAddressDetailsNotVisible(field)
                    : page.IsAddressDetailsVisible(field));
            }
        }

        [Then(@"Activity details on view activty page are following")]
        public void CheckActivityDetails(Table table)
        {
            var details = table.CreateInstance<EditActivityDetails>();
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");

            Verify.That(this.driverContext,
                () => Assert.Equal(details.ActivityStatus, page.Status),
                () => Assert.Equal(details.MarketAppraisalPrice.ToString("N2") + " GBP", page.MarketAppraisalPrice),
                () => Assert.Equal(details.RecommendedPrice.ToString("N2") + " GBP", page.RecommendedPrice),
                () => Assert.Equal(details.VendorEstimatedPrice.ToString("N2") + " GBP", page.VendorEstimatedPrice));
        }

        [Then(@"Attachment is displayed on view activity page")]
        public void CheckIfAttachmentIsDisplayed(Table table)
        {
            Attachment actual =
                this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").GetAttachmentDetails();
            var expected = table.CreateInstance<Attachment>();
            expected.Date = DateTime.UtcNow.ToString("dd-MM-yyyy");

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"Attachment details on attachment preview page are the same like on view activity page")]
        public void ChackAttachmentDetails()
        {
            Attachment actual =
                this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").PreviewAttachment.GetAttachmentDetails();
            Attachment expected = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").GetAttachmentDetails();
            expected.User = "John Smith";

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
