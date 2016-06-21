namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewActivitySteps
    {
        private const string Format = "dd-MM-yyyy";
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
            this.page.WaitForSidePanelToHide(60);
        }

        [When(@"User clicks attachment card on view activity page")]
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

        [When(@"User clicks (.*) offer details on view activity page")]
        public void OpenOffersDetails(int position)
        {
            this.page.OpenOfferDetails(position)
                .WaitForSidePanelToShow();
        }

        [When(@"User edits lead negotiator next call to (.*) days from current day on view activity page")]
        public void UpdateLeadNegotiatorNextCall(int days)
        {
            this.page.EditLeadNegotiatorNextCall(days);
        }

        [When(@"User edits secondary negotiator next call on view activity page")]
        public void UpdateSecondaryNegotiatorsNextCall(Table table)
        {
            IEnumerable<Negotiator> secondaryNegotiatorsNextCall = table.CreateSet<Negotiator>();
            foreach (Negotiator negotiator in secondaryNegotiatorsNextCall)
            {
                this.page.EditSecondaryNegotiatorNextCall(negotiator.Name, int.Parse(negotiator.NextCall));
            }
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

        [Then(@"Attachment should be displayed on view activity page")]
        public void CheckIfAttachmentIsDisplayed(Table table)
        {
            Attachment actual = this.page.AttachmentDetails;
            var expected = table.CreateInstance<Attachment>();
            expected.Date = DateTime.UtcNow.ToString(Format);

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"Attachment details on attachment preview page are the same like on view activity page")]
        public void ChackAttachmentDetails()
        {
            Attachment actual = this.page.PreviewAttachment.GetAttachmentDetails();
            Attachment expected = this.page.AttachmentDetails;
            expected.User = "John Smith";

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"View activity page should be displayed")]
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

        [Then(@"Secondary negotiators are set on view activity page")]
        public void CheckSecondaryUsers(Table table)
        {
            List<Negotiator> expectedSecondary = table.CreateSet<Negotiator>().ToList();
            foreach (Negotiator secondaryNegotiator in expectedSecondary)
            {
                secondaryNegotiator.NextCall = secondaryNegotiator.NextCall != "-"
                    ? DateTime.UtcNow.AddDays(int.Parse(secondaryNegotiator.NextCall)).ToString(Format)
                    : "-";
            }

            List<Negotiator> actual = this.page.GetSecondaryNegotiatorsData();

            expectedSecondary.Should().Equal(actual, (n1, n2) => n1.Name.Equals(n2.Name) && n1.NextCall.Equals(n2.NextCall));
        }

        [Then(@"Offer should be displayed on view activity page")]
        public void CheckIfOfferDisplayed()
        {
            Assert.Equal(1, this.page.OffersNumber);
        }

        [Then(@"Offer details on (.*) position on view activity page are same as the following")]
        public void CheckOffer(int position, Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            List<string> actualDetails = this.page.GetOfferDetails(position);

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, actualDetails[0]),
                () => Assert.Equal(expectedDetails.Offer, actualDetails[1]),
                () => Assert.Equal(expectedDetails.Status, actualDetails[2]));
        }

        [Then(@"Offer details on view activity page are same as the following")]
        public void CheckOfferInDetailsPanel(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();

            var offer = this.scenarioContext.Get<OfferData>("Offer");

            expectedDetails.OfferDate = offer.OfferDate;
            expectedDetails.ExchangeDate = offer.ExchangeDate;
            expectedDetails.CompletionDate = offer.CompletionDate;

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, this.page.OfferPreview.Details),
                () => Assert.Equal(expectedDetails.Status, this.page.OfferPreview.Status),
                () => Assert.Equal(expectedDetails.Offer, this.page.OfferPreview.Offer),
                () => Assert.Equal(expectedDetails.OfferDate, this.page.OfferPreview.Date),
                () => Assert.Equal(expectedDetails.SpecialConditions, this.page.OfferPreview.SpecialConditions),
                () => Assert.Equal(expectedDetails.Negotiator, this.page.OfferPreview.Negotiator),
                () => Assert.Equal(expectedDetails.ExchangeDate, this.page.OfferPreview.ProposedexchangeDate),
                () => Assert.Equal(expectedDetails.CompletionDate, this.page.OfferPreview.ProposedCompletionDate));
        }

        [Then(@"Lead negotiator next call is set to (.*) days from current day on view activity page")]
        public void GetDefaultNextCallDateForLeadNegotiator(int day)
        {
            Assert.Equal(DateTime.UtcNow.AddDays(day).ToString(Format), this.page.LeadNegotiatorNextCall);
        }

        [Then(@"Departments are displayed on view activity page")]
        public void CheckDepartment(Table table)
        {
            List<Department> expectedDepartments = table.CreateSet<Department>().ToList();
            List<Department> actualDepartments = this.page.Departments;
            expectedDepartments.Should().Equal(actualDepartments, (d1, d2) => d1.Name.Equals(d2.Name));
        }
    }
}
