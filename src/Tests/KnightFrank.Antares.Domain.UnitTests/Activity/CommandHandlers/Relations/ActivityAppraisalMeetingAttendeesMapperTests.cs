namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers.Relations
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("ActivityAppraisalMeetingAttendeesMapper")]
    public class ActivityAppraisalMeetingAttendeesMapperTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [InlineAutoMoqData(0, 0)]
        [InlineAutoMoqData(0, 2)]
        [InlineAutoMoqData(1, 0)]
        [InlineAutoMoqData(2, 2)]
        [InlineAutoMoqData(3, 5)]
        public void Given_ValidCommand_When_Handling_Then_ShouldAssignAttendees(
            int existingAttendees,
            int attendeesInCommand,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            ActivityAppraisalMeetingAttendeesMapper mapper,
            IFixture fixture)
        {
            // Arrange
            var command = new UpdateActivityCommand();
            command.AppraisalMeeting.Attendees =
                Enumerable.Range(0, attendeesInCommand)
                          .Select(
                              i =>
                                  i % 2 == 0
                                      ? new UpdateActivityAttendee { UserId = Guid.NewGuid() }
                                      : new UpdateActivityAttendee { ContactId = Guid.NewGuid() })
                          .ToList();
            var activity = fixture.Create<Activity>();
            activity.ActivityAttendees =
                Enumerable.Range(0, existingAttendees)
                          .Select(
                              i =>
                                  i % 2 == 0
                                      ? new ActivityAttendee { UserId = Guid.NewGuid() }
                                      : new ActivityAttendee { ContactId = Guid.NewGuid() })
                          .ToList();
            activity.ActivityUsers =
                command.AppraisalMeeting.Attendees.Where(x => x.UserId.HasValue).Select(x =>
                {
                    var user = fixture.Create<ActivityUser>();
                    user.UserId = x.UserId.Value;
                    return user;
                }).ToList();
            activity.Contacts =
                command.AppraisalMeeting.Attendees.Where(x => x.ContactId.HasValue).Select(x =>
                {
                    var contact = fixture.Create<Contact>();
                    contact.Id = x.ContactId.Value;
                    return contact;
                }).ToList();

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            activity.ActivityAttendees.Select(x => new { x.UserId, x.ContactId })
                    .Should()
                    .BeEquivalentTo(command.AppraisalMeeting.Attendees.Select(x => new { x.UserId, x.ContactId }));
        }
    }
}
