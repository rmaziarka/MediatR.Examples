namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
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

        [When(@"User navigates to view activity page with id")]
        public void OpenViewActivityPageWithId()
        {
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            ViewActivityPage page = new ViewActivityPage(this.driverContext).OpenViewActivityPageWithId(activityId.ToString());
            this.scenarioContext.Set(page, "ViewActivityPage");
        }

        [When(@"User clicks property details link on view activity page")]
        public void OpenPreviewPropertyPage()
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").ClickDetailsLink().WaitForSidePanelToShow();
        }

        [When(@"User clicks view property link on property preview page")]
        public void OpenViewPropertyPage()
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");
            this.scenarioContext.Set(page.PropertyPreview.OpenViewPropertyPage(), "ViewPropertyPage");
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
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").OpenAttachFilePanel().WaitForSidePanelToShow();
        }

        [When(@"User adds (.*) file with (.*) type on attach file page")]
        public void SelectAttachmentType(string file, string type)
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");
            page.AttachFile.SelectType(type)
                .AddFiletoAttachment(file)
                .SaveAttachment();
            page.WaitForSidePanelToHide(BaseConfiguration.LongTimeout);
        }

        [When(@"User clicks attachment details link on view activity page")]
        public void OpenAttachmentPreview()
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").OpenAttachmentPreview().WaitForSidePanelToShow();
        }

        [When(@"User clicks (.*) viewings details link on view activity page")]
        public void OpenViewingsDetails(int position)
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").OpenViewingDetails(position);
        }

        [When(@"User clicks view requirement on view activity page")]
        public void ClickViewActivity()
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").ViewingDetails.ClickViewLink();
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

        [Then(@"View activity page is displayed")]
        public void CheckIfViewActivityPresent()
        {
            var page = new ViewActivityPage(this.driverContext);
            Assert.True(page.IsViewActivityFormPresent());
            this.scenarioContext.Set(page, "ViewActivityPage");
        }

        [Then(@"Viewing details on (.*) position on view activity page are same as the following")]
        public void CheckViewing(int position, Table table)
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");
            var expectedDetails = table.CreateInstance<ViewingData>();
            List<string> actualDetails = page.GetViewingDetails(position);

            Verify.That(this.driverContext, () => Assert.Equal(page.ViewingsNumber, 1),
                () => Assert.Equal(expectedDetails.Date, actualDetails[0]),
                () => Assert.Equal(expectedDetails.Name, actualDetails[1]),
                () => Assert.Equal(expectedDetails.Time, actualDetails[2]));
        }

        [Then(@"Viewing details on view activity page are same as the following")]
        public void CheckViewingInDetailsPanel(Table table)
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");
            var expectedDetails = table.CreateInstance<ViewingDetails>();

            List<string> attendees = expectedDetails.Attendees.Split(';').ToList();

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Name, page.ViewingDetails.Details),
                () => Assert.Equal(expectedDetails.Date + ", " + expectedDetails.StartTime + " - " + expectedDetails.EndTime, page.ViewingDetails.Date),
                () => Assert.Equal(expectedDetails.Negotiator, page.ViewingDetails.Negotiator),
                () => Assert.Equal(attendees, page.ViewingDetails.Attendees),
                () => Assert.Equal(expectedDetails.InvitationText, page.ViewingDetails.InvitationText),
                () => Assert.Equal(expectedDetails.PostViewingComment, page.ViewingDetails.PostViewingComment));
        }

        [Then(@"User closes attachment preview page on view activity page")]
        public void CloseAttachmentPreviewPanel()
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");
            page.PreviewAttachment.CloseAttachmentPreviewPage();
            page.WaitForSidePanelToHide();
        }
    }
}
