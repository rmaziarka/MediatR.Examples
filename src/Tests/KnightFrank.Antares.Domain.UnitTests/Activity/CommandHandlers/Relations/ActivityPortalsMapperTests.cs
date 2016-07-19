namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Portal;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("ActivityPortalsMapper")]
    public class ActivityPortalsMapperTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [InlineAutoMoqData(0, 0)]
        [InlineAutoMoqData(0, 2)]
        [InlineAutoMoqData(1, 0)]
        [InlineAutoMoqData(2, 2)]
        [InlineAutoMoqData(3, 5)]
        public void Given_ValidCommand_When_Handling_Then_ShouldAssignPortals(
            int existingPortals,
            int portalsInCommand,
            [Frozen] Mock<IGenericRepository<Portal>> portalRepository,
            ActivityPortalsMapper mapper,
            IFixture fixture)
        {
            // Arrange
            List<UpdateActivityPortal> portalsToUpdate = Enumerable.Range(0, portalsInCommand).Select(i => new UpdateActivityPortal { Id = Guid.NewGuid() }).ToList();
            var command = new UpdateActivityCommand { AdvertisingPortals = portalsToUpdate };
            var activity = fixture.Create<Activity>();
            activity.AdvertisingPortals = Enumerable.Range(0, existingPortals).Select(i => new Portal { Id = Guid.NewGuid() }).ToList();

            List<Portal> newPortals = portalsToUpdate.Select(up => new Portal { Id = up.Id }).ToList();
            var newPortalIndex = 0;
            portalRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(() => newPortals[newPortalIndex++]);

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            activity.AdvertisingPortals.Should().BeEquivalentTo(newPortals);
        }
    }
}
