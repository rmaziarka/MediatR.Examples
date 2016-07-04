namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers.Relations
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
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
            [Frozen] Mock<IGenericRepository<ActivityAttendee>> activityAttendeeRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            ActivityAppraisalMeetingAttendeesMapper mapper,
            IFixture fixture)
        {
            // Arrange
            var activity = fixture.Create<Activity>();

            activityAttendeeRepository.Setup(x => x.Delete(It.IsAny<ActivityAttendee>()))
                                      .Callback<ActivityAttendee>(x => activity.AppraisalMeetingAttendees.Remove(x));

            int expectedDeletesCount = attendeesInCommand != 0 ? existingAttendees : 0;

            var command = new UpdateActivityCommand();
            command.AppraisalMeetingAttendeesList =
                Enumerable.Range(0, attendeesInCommand)
                          .Select(
                              i =>
                                  i % 2 == 0
                                      ? new UpdateActivityAttendee { UserId = Guid.NewGuid() }
                                      : new UpdateActivityAttendee { ContactId = Guid.NewGuid() })
                          .ToList();

            activity.AppraisalMeetingAttendees =
                Enumerable.Range(0, existingAttendees)
                          .Select(
                              i =>
                                  i % 2 == 0
                                      ? new ActivityAttendee { UserId = Guid.NewGuid() }
                                      : new ActivityAttendee { ContactId = Guid.NewGuid() })
                          .ToList();

            activity.ActivityUsers =
                command.AppraisalMeetingAttendeesList.Where(x => x.UserId.HasValue).Select(x =>
                {
                    var user = fixture.Create<ActivityUser>();
                    user.UserId = x.UserId.Value;
                    return user;
                }).ToList();

            activity.Contacts =
                command.AppraisalMeetingAttendeesList.Where(x => x.ContactId.HasValue).Select(x =>
                {
                    var contact = fixture.Create<Contact>();
                    contact.Id = x.ContactId.Value;
                    return contact;
                }).ToList();

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            activity.AppraisalMeetingAttendees.Select(x => new { x.UserId, x.ContactId })
                    .Should()
                    .BeEquivalentTo(command.AppraisalMeetingAttendeesList.Select(x => new { x.UserId, x.ContactId }));

            activityAttendeeRepository.Verify(x => x.Delete(It.IsAny<ActivityAttendee>()), Times.Exactly(expectedDeletesCount));
        }
    }
}
