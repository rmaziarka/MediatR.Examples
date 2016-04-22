namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers
{
    using System;

    using FluentAssertions;
    
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("UpdateActivityCommandHandler")]
    public class UpdateActivityCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ShouldUpdateActivity(
            [Frozen] Mock<IGenericRepository<Activity>> activityRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandHandler handler,
            Activity activity)
        {
            activityRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(activity);

            handler.Handle(command);

            activity.ShouldBeEquivalentTo(
                command,
                options =>
                options.Including(x => x.ActivityStatusId)
                       .Including(x => x.MarketAppraisalPrice)
                       .Including(x => x.RecommendedPrice)
                       .Including(x => x.VendorEstimatedPrice));
        }
    }
}