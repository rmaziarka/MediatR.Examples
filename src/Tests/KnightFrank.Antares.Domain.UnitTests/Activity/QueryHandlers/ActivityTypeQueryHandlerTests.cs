namespace KnightFrank.Antares.Domain.UnitTests.Activity.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;
    using KnightFrank.Antares.Domain.Activity.QueryHandlers;
    using KnightFrank.Antares.Domain.Activity.QueryResults;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ActivityTypeQueryHandler")]
    [Trait("FeatureTitle", "ActivityType")]
    public class ActivityTypeQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistingActivityType_When_Handling_Then_ShouldReturnNotNullValue(
            Guid propertyTypeId,
            string countryCode,
            [Frozen] Mock<IReadGenericRepository<ActivityTypeDefinition>> activityTypeRepository,
            ActivityTypeQueryHandler handler)
        {
            // Arrange
            var query = new ActivityTypeQuery { CountryCode = countryCode, PropertyTypeId = propertyTypeId };
            IQueryable<ActivityTypeDefinition> activityTypeDefinitions = new[]
            {
                new ActivityTypeDefinition
                {
                    ActivityType = new ActivityType { Id = Guid.NewGuid() },
                    Id = Guid.NewGuid(),
                    Country = new Country { IsoCode = countryCode },
                    PropertyTypeId = propertyTypeId,
                    Order = 1
                }
            }.AsQueryable();

            activityTypeRepository.Setup(r => r.Get())
                                  .Returns(activityTypeDefinitions);

            // Act
            IEnumerable<ActivityTypeQueryResult> activityTypes = handler.Handle(query).ToList();

            // Assert
            activityTypes.Should().HaveCount(1);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingActivityType_When_Handling_Then_ShouldReturnActivityTypeWithCorrectPropertyTypeAndCountryCode(
            Guid propertyTypeId,
            string countryCode,
            [Frozen] Mock<IReadGenericRepository<ActivityTypeDefinition>> activityTypeRepository,
            ActivityTypeQueryHandler handler)
        {
            // Arrange
            var query = new ActivityTypeQuery { CountryCode = countryCode, PropertyTypeId = propertyTypeId };
            IQueryable<ActivityTypeDefinition> activityTypeDefinitions = new[]
            {
                new ActivityTypeDefinition
                {
                    ActivityType = new ActivityType { Id = Guid.NewGuid() },
                    Id = Guid.NewGuid(),
                    Country = new Country { IsoCode = countryCode },
                    PropertyTypeId = propertyTypeId,
                    Order = 1
                },
                new ActivityTypeDefinition
                {
                    ActivityType = new ActivityType { Id = Guid.NewGuid() },
                    Id = Guid.NewGuid(),
                    Country = new Country { IsoCode = countryCode },
                    PropertyTypeId = Guid.NewGuid(),
                    Order = 2
                },
                new ActivityTypeDefinition
                {
                    ActivityType = new ActivityType { Id = Guid.NewGuid() },
                    Id = Guid.NewGuid(),
                    Country = new Country { IsoCode = String.Empty },
                    PropertyTypeId = propertyTypeId,
                    Order = 3
                }
            }.AsQueryable();

            activityTypeRepository.Setup(r => r.Get())
                                  .Returns(activityTypeDefinitions);

            // Act
            IEnumerable<ActivityTypeQueryResult> activityTypes = handler.Handle(query).ToList();

            // Assert
            activityTypes.Should().HaveCount(1);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingActivityType_When_Handling_Then_ShouldReturnActivityTypeInCorrectOrder(
            Guid propertyTypeId,
            string countryCode,
            [Frozen] Mock<IReadGenericRepository<ActivityTypeDefinition>> activityTypeRepository,
            ActivityTypeQueryHandler handler)
        {
            // Arrange
            var query = new ActivityTypeQuery { CountryCode = countryCode, PropertyTypeId = propertyTypeId };
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Guid id3 = Guid.NewGuid();

            IQueryable<ActivityTypeDefinition> activityTypeDefinitions = new[]
            {
                new ActivityTypeDefinition
                {
                    ActivityType = new ActivityType { Id = id3 },
                    Id = Guid.NewGuid(),
                    Country = new Country { IsoCode = countryCode },
                    PropertyTypeId = propertyTypeId,
                    Order = 3
                },
                new ActivityTypeDefinition
                {
                    ActivityType = new ActivityType { Id = id1 },
                    Id = Guid.NewGuid(),
                    Country = new Country { IsoCode = countryCode },
                    PropertyTypeId = propertyTypeId,
                    Order = 1
                },
                new ActivityTypeDefinition
                {
                    ActivityType = new ActivityType { Id = id2 },
                    Id = Guid.NewGuid(),
                    Country = new Country { IsoCode = countryCode },
                    PropertyTypeId = propertyTypeId,
                    Order = 2
                }
            }.AsQueryable();

            activityTypeRepository.Setup(r => r.Get())
                                  .Returns(activityTypeDefinitions);

            // Act
            IEnumerable<ActivityTypeQueryResult> activityTypes = handler.Handle(query).ToList();

            // Assert
            activityTypes.Should().HaveCount(3);
            activityTypes.ElementAt(0).Id.Should().Be(id1);
            activityTypes.ElementAt(1).Id.Should().Be(id2);
            activityTypes.ElementAt(2).Id.Should().Be(id3);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NonExistingActivityTypeDefinition_When_Handling_Then_ShouldReturnDomainException(
            Guid propertyTypeId,
            string countryCode,
            [Frozen] Mock<IReadGenericRepository<ActivityTypeDefinition>> activityTypeRepository,
            ActivityTypeQueryHandler handler)
        {
            // Arrange
            var query = new ActivityTypeQuery { CountryCode = countryCode, PropertyTypeId = propertyTypeId };

            IQueryable<ActivityTypeDefinition> activityTypeDefinitions = new ActivityTypeDefinition[0].AsQueryable();

            activityTypeRepository.Setup(r => r.Get())
                                  .Returns(activityTypeDefinitions);

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => { handler.Handle(query); });
        }
    }
}
