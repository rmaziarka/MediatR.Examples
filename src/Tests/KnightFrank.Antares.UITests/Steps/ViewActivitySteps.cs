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
        private ViewActivityPage page;

        public ViewActivitySteps(ScenarioContext scenarioContext)
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

        [When(@"User navigates to view activity page with id")]
        public void OpenViewActivityPageWithId()
        {
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            this.page = new ViewActivityPage(this.driverContext).OpenViewActivityPageWithId(activityId.ToString());
        }

        [When(@"User clicks property details link on view activity page")]
        public void OpenPreviewPropertyPage()
        {
            this.page.ClickDetailsLink().WaitForSidePanelToShow();
        }

        [When(@"User clicks view property link from property on view activity page")]
        public void OpenViewPropertyPage()
        {
            this.page.PropertyPreview.OpenViewPropertyPage();
        }

        [When(@"User clicks edit button on view activity page")]
        public void EditActivity()
        {
            this.page.EditActivity();
        }

        [When(@"User clicks add attachment button on view activity page")]
        public void OpenAttachFilePanel()
        {
            this.page.OpenAttachFilePanel().WaitForSidePanelToShow();
        }

        [When(@"User adds (.*) file with (.*) type on attach file page")]
        public void SelectAttachmentType(string file, string type)
        {
            this.page.AttachFile.SelectType(type)
                .AddFiletoAttachment(file)
                .SaveAttachment();
            this.page.WaitForSidePanelToHide(BaseConfiguration.LongTimeout);
        }

        [When(@"User clicks attachment details link on view activity page")]
        public void OpenAttachmentPreview()
        {
            this.page.OpenAttachmentPreview().WaitForSidePanelToShow();
        }

        [When(@"User clicks (.*) viewings details link on view activity page")]
        public void OpenViewingsDetails(int position)
        {
            this.page.OpenViewingDetails(position);
        }

        [When(@"User clicks view requirement from viewing on view activity page")]
        public void ClickViewActivity()
        {
            this.page.ViewingDetails.ClickViewLink();
        }

        [Then(@"Attachment (.*) should be downloaded")]
        public void ThenAttachmentShouldBeDownloaded(string attachmentName)
        {
            FileInfo fileInfo = this.page.PreviewAttachment.GetDownloadedAttachmentInfo();

            Verify.That(this.driverContext,
                () => Assert.Equal(attachmentName.ToLower(), fileInfo.Name),
                () => Assert.Equal("." + attachmentName.Split('.')[1], fileInfo.Extension));
        }

        [Then(@"Address details on view activity page are following")]
        public void CheckViewActivityAddressDetails(Table table)
        {
            foreach (string field in table.Rows.SelectMany(row => row.Values))
            {
                Assert.True(field.Equals(string.Empty)
                    ? this.page.IsAddressDetailsNotVisible(field)
                    : this.page.IsAddressDetailsVisible(field));
            }
        }

        [Then(@"Activity details on view activty page are following")]
        public void CheckActivityDetails(Table table)
        {
            var details = table.CreateInstance<EditActivityDetails>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.ActivityStatus, this.page.Status),
                () => Assert.Equal(details.MarketAppraisalPrice.ToString("N2") + " GBP", this.page.MarketAppraisalPrice),
                () => Assert.Equal(details.RecommendedPrice.ToString("N2") + " GBP", this.page.RecommendedPrice),
                () => Assert.Equal(details.VendorEstimatedPrice.ToString("N2") + " GBP", this.page.VendorEstimatedPrice));
        }

        [Then(@"Attachment is displayed on view activity page")]
        public void CheckIfAttachmentIsDisplayed(Table table)
        {
            Attachment actual = this.page.GetAttachmentDetails();
            var expected = table.CreateInstance<Attachment>();
            expected.Date = DateTime.UtcNow.ToString("dd-MM-yyyy");

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"Attachment details on attachment preview page are the same like on view activity page")]
        public void ChackAttachmentDetails()
        {
            Attachment actual = this.page.PreviewAttachment.GetAttachmentDetails();
            Attachment expected = this.page.GetAttachmentDetails();
            expected.User = "John Smith";

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"View activity page is displayed")]
        public void CheckIfViewActivityPresent()
        {
            Assert.True(this.page.IsViewActivityFormPresent());
        }

        [Then(@"Viewing details on (.*) position on view activity page are same as the following")]
        public void CheckViewing(int position, Table table)
        {
            var expectedDetails = table.CreateInstance<ViewingData>();
            List<string> actualDetails = this.page.GetViewingDetails(position);

            Verify.That(this.driverContext, () => Assert.Equal(this.page.ViewingsNumber, 1),
                () => Assert.Equal(expectedDetails.Date, actualDetails[0]),
                () => Assert.Equal(expectedDetails.Name, actualDetails[1]),
                () => Assert.Equal(expectedDetails.Time, actualDetails[2]));
        }

        [Then(@"Viewing details on view activity page are same as the following")]
        public void CheckViewingInDetailsPanel(Table table)
        {
            var expectedDetails = table.CreateInstance<ViewingDetails>();

            List<string> attendees = expectedDetails.Attendees.Split(';').ToList();

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Name, this.page.ViewingDetails.Details),
                () => Assert.Equal(expectedDetails.Date + ", " + expectedDetails.StartTime + " - " + expectedDetails.EndTime, this.page.ViewingDetails.Date),
                () => Assert.Equal(expectedDetails.Negotiator, this.page.ViewingDetails.Negotiator),
                () => Assert.Equal(attendees, this.page.ViewingDetails.Attendees),
                () => Assert.Equal(expectedDetails.InvitationText, this.page.ViewingDetails.InvitationText),
                () => Assert.Equal(expectedDetails.PostViewingComment, this.page.ViewingDetails.PostViewingComment));
        }

        [Then(@"User closes attachment preview page on view activity page")]
        public void CloseAttachmentPreviewPanel()
        {
            this.page.PreviewAttachment.CloseAttachmentPreviewPage();
            this.page.WaitForSidePanelToHide();
        }

        [Then(@"(.*) is set as lead negotiator on view activity page")]
        public void CheckLeadNegotiator(string expectedLead)
        {
            Assert.Equal(expectedLead, this.page.LeadNegotiator);
        }

        [Then(@"Secondary users are set on view activity page")]
        public void CheckSecondaryUsers(Table table)
        {
            List<Negotiator> expectedSecondary = table.CreateSet<Negotiator>().ToList();
            List<Negotiator> actualSecondary = this.page.SecondaryNegotiators;
            actualSecondary.Should().Equal(expectedSecondary, (c1, c2) =>
                c1.Name.Equals(c2.Name));
        }
    }
}
