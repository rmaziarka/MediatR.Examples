namespace KnightFrank.Antares.Domain.UnitTests.Activity.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryHandlers;
    using KnightFrank.Antares.Domain.Activity.QueryResults;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ActivitiesQueryHandler")]
    [Trait("FeatureTitle", "Activity")]
    public class ActivitiesQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ActivitiesExists_When_Handling_Then_ShouldReturnNotNullValue(
            [Frozen] Mock<IReadGenericRepository<Activity>> activityTypeRepository,
            ActivitiesQueryHandler handler)
        {
            // Arrange
            var query = new ActivitiesFilterQuery();
            IQueryable<Activity> activities = new[]
            {
                new Activity
                {
                    Id = Guid.NewGuid(),
                    Property = new Property
                    {
                        Address = new Address { PropertyName = "Name", PropertyNumber = "Number", Line2 = "Address line 2" }
                    }
                }
            }.AsQueryable();

            activityTypeRepository.Setup(r => r.Get())
                                  .Returns(activities);

            // Act
            IEnumerable<ActivitiesQueryResult> result = handler.Handle(query);

            // Assert
            result.Should().HaveCount(1);
        }
    }
}
