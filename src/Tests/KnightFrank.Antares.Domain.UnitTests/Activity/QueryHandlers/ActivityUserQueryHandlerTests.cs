namespace KnightFrank.Antares.Domain.UnitTests.Activity.QueryHandlers
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryHandlers;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ActivityUserQueryHandler")]
    [Trait("FeatureTitle", "Activity")]
    public class ActivityUserQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ActivitiesExists_When_Handling_Then_ShouldReturnNotNullValue(
            [Frozen] Mock<IReadGenericRepository<ActivityUser>> activityUserRepository,
            ActivityUserQuery query,
            ActivityUserQueryHandler handler,
            IFixture fixture)
        {
            // Arrange
            ActivityUser expectedActivityUser = this.CreateActivityUser(query.Id, query.ActivityId, fixture);
            activityUserRepository.Setup(r => r.Get())
                                  .Returns(
                                      new[]
                                          {
                                              expectedActivityUser,
                                              this.CreateActivityUser(Guid.NewGuid(), query.ActivityId, fixture),
                                              this.CreateActivityUser(Guid.NewGuid(), Guid.NewGuid(), fixture)
                                          }.AsQueryable());

            // Act
            ActivityUser result = handler.Handle(query);

            // Assert
            result.ShouldBeEquivalentTo(expectedActivityUser);
        }

        private ActivityUser CreateActivityUser(Guid activityUserId, Guid activityId, IFixture fixture)
        {
            return fixture.Build<ActivityUser>().With(au => au.Id, activityUserId).With(au => au.ActivityId, activityId).Create();
        }
    }
}
