namespace KnightFrank.Antares.Domain.UnitTests.Activity.QueryHandlers
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryHandlers;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("Activity")]
    [Trait("FeatureTitle", "Activity")]
    public class ActivityQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistingActivityWithId_When_Handling_Then_ShouldReturnNotNullValue(
            Guid searchByActivityId,
            [Frozen] Mock<IReadGenericRepository<Activity>> activityRepository,
            ActivityQueryHandler handler,
            IFixture fixture)
        {
            // Arrange
            Activity expectedActivity =
                fixture.Build<Activity>().With(a => a.Id, searchByActivityId).Without(a => a.Property).Create();
            var query = new ActivityQuery { Id = searchByActivityId };
            activityRepository.Setup(r => r.Get()).Returns(new[] { expectedActivity }.AsQueryable());

            // Act
            Activity activity = handler.Handle(query);

            // Assert
            activity.Should().NotBeNull();
            activity.ShouldBeEquivalentTo(expectedActivity, options => options.IncludingProperties().ExcludingMissingMembers());
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingActivityWithId_When_Handling_Then_ShouldReturnNull(
            ActivityQuery query,
            [Frozen] Mock<IReadGenericRepository<Activity>> activityRepository,
            ActivityQueryHandler handler)
        {
            // Arrange
            activityRepository.Setup(r => r.Get()).Returns(new Activity[] { }.AsQueryable());

            // Act
            Activity activity = handler.Handle(query);

            // Assert
            activity.Should().BeNull();
        }
    }
}
